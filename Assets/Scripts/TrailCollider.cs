using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Flat;
 
//Add this script to a new gameobject at 0,0,0
//make sure the gameobject position does not change.
public class TrailCollider : MonoBehaviour
{
    public TrailRenderer _tr; //assign the trailrenderer in editor.

    Vector2 prevPoint;
    Vector2 currPoint;
    private int[] triangles;

    public AudioClip splat;
    public AudioClip ding;
    float volume = 0.5f;

    bool crossed = false;

    AudioSource audioSource;
    public FollowTouch followTouch;

    public bool isPaused = false;

    // Web Gen Code
    public float webInterval = 2.0f;
    public float webCount = 0f;
    public GameObject webPixel;
    public GameObject testPixel;
    public SpriteRenderer webRenderer;
    public float pixelSize;
    public float pixelScale = .1f;
    public Vector3 lastPixel = new Vector3(0f, 0f);

    public float outerTop;
    public float outerBottom;
    public float outerLeft;
    public float outerRight;

    public float innerTop;
    public float innerBottom;
    public float innerLeft;
    public float innerRight;

    public int updateCount = 0;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pixelSize = webRenderer.size.x;
        SetBounds();
    }

    void Update()
    {
        updateCount++;

        Vector3 currPosition = transform.position;
        if (currPosition.x <= outerLeft | currPosition.x >= outerRight | currPosition.y <= outerBottom | currPosition.y >= outerTop)
        {
            Debug.Log("New pixel");
            PlacePixels();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            followTouch.deploying = true;
            UpdateCollider();
        }
        else if (!isPaused) {
            _tr.Clear();
            followTouch.deploying = false;
        }
    }

    void UpdateCollider()
    {
        Vector3[] pointsInTrailRenderer3d = new Vector3[_tr.positionCount]; 
        _tr.GetPositions(pointsInTrailRenderer3d);
        Vector2[] pointsInTrailRenderer = ConvertArray(pointsInTrailRenderer3d); 

        if (pointsInTrailRenderer.Length > 1)
        {

            Vector2 newPoint = pointsInTrailRenderer.Last();
            double newX = newPoint.x;
            double newY = newPoint.y;


            Vector2[] pointsWithoutNewest = pointsInTrailRenderer.SkipLast(1).ToArray();
            Vector2 prevPoint = pointsWithoutNewest.Last();

            double prevX = prevPoint.x;
            double prevY = prevPoint.y;


            // Check if there was a hover, skip segment check if there was a hover
            if (newX != prevX | newY != prevY)
            {
                // Check if each of the previous segments intersect with current segment
                Vector2 tailPoint = pointsWithoutNewest[0];
                Vector2 headPoint = pointsWithoutNewest[0];
                for (int i=0; i < pointsWithoutNewest.Length; i++)
                {
                    tailPoint = headPoint;
                    headPoint = pointsWithoutNewest[i];

                    // Don't check segment if head and tail point are identical
                    // Also don't check segment if head and prev point are identical
                    if ((headPoint.x != tailPoint.x | headPoint.y != tailPoint.y) & (headPoint.x != prevPoint.x | headPoint.y != prevPoint.y))
                    {
                        crossed = intersect(newPoint, prevPoint, headPoint, tailPoint);
                        if (crossed)
                        {
                            
                            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                            Vector2 intersection = CreateIntersection(newPoint, prevPoint, headPoint, tailPoint);
                            Vector2[] rawLoop = pointsInTrailRenderer;
                            rawLoop[rawLoop.Length - 1] = intersection;

                            Vector2[] finalLoop = CreateLoopArray(rawLoop, i - 1);
                            bool[] killEnemies = IsInsideWeb(enemies, finalLoop);
                            // AudioSource.PlayClipAtPoint(splat, transform.position, volume);
                            audioSource.PlayOneShot(splat, volume);
                            KillEnemies(enemies, killEnemies);

                            crossed = false;
                            _tr.Clear();
                        }
                    }
                }
            }
        }
    }

    void KillEnemies(GameObject[] enemies, bool[] inWeb)
    {
        for (int i=0; i < inWeb.Length; i++)
        {
            if (inWeb[i])
            {
                Destroy(enemies[i]);
                ScoreManager.instance.AddPoint();
                // AudioSource.PlayClipAtPoint(ding, transform.position, volume);
                audioSource.PlayOneShot(ding, volume);
            }
        }
    }

    bool[] IsInsideWeb(GameObject[] enemies, Vector2[] polygonPoints)
    {
        bool[] final = Enumerable.Repeat(false, enemies.Length).ToArray();
        float xMax = 20f;

        for (int i=0; i < polygonPoints.Length; i++)
        {
            int prevIndex = i - 1 < 0 ? polygonPoints.Length - 1 : i - 1;
            Vector2 headPoint = polygonPoints[i];
            Vector2 tailPoint = polygonPoints[prevIndex];

            for (int j=0; j < enemies.Length; j++)
            {
                GameObject enemyGameObject = enemies[j];
                float enemyX = enemyGameObject.transform.position.x;
                float enemyY = enemyGameObject.transform.position.y;
                Vector2 enemyPoint = new Vector2(enemyX, enemyY);
                Vector2 enemyRayPoint = new Vector2(xMax, enemyY);
                if (intersect(headPoint, tailPoint, enemyPoint, enemyRayPoint))
                {
                    final[j] = !final[j];
                }
            }
        }

        return final;
    }

    Vector2 CreateIntersection(Vector2 newPoint, Vector2 prevPoint, Vector2 headPoint, Vector2 tailPoint)
    {
        //Line1
        float A1 = prevPoint.y - newPoint.y;
        float B1 = newPoint.x - prevPoint.x;
        float C1 = A1*newPoint.x + B1*newPoint.y;

        //Line2
        float A2 = tailPoint.y - headPoint.y;
        float B2 = headPoint.x - tailPoint.x;
        float C2 = A2 * headPoint.x + B2 * headPoint.y;

        float delta = A1 * B2 - A2 * B1;

        if (delta == 0) 
            throw new ArgumentException("Lines are parallel");

        float x = (B2 * C1 - B1 * C2) / delta;
        float y = (A1 * C2 - A2 * C1) / delta;

        Vector2 intersection = new Vector2(x,y);
        
        return intersection;
    }

    // rawPoints: all points in current trail renderer
    // cutOff: the index of tailPoint at time of intersection
    // intersection: vertex representing point of intersection
    Vector2[] CreateLoopArray(Vector2[] rawPoints, int cutOff)
    {
        // Create new array without tail point and all points before it
        Vector2[] minusTailing = rawPoints.Skip(cutOff).ToArray();

        // Filter out any repeat points
        HashSet<Vector2> pointsSet = new HashSet<Vector2>(minusTailing);

        // Convert to array
        Vector2[] uniquePoints = new Vector2[pointsSet.Count];
        pointsSet.CopyTo(uniquePoints);

        return uniquePoints;
    }

    void PlacePixels()
    {
        float xDiff = transform.position.x - lastPixel.x;
        float yDiff = transform.position.y - lastPixel.y;

        float xRemainder = xDiff % 1;
        float yRemainder = yDiff % 1;

        int xCoord = (int)Math.Round(xDiff / pixelScale, 0);
        int yCoord = (int)Math.Round(yDiff / pixelScale, 0);

        List<Vector2> bresenhamPixels = BresenhamPixels(xCoord, yCoord);
        
        if (xRemainder > .5f | yRemainder > .5f)
        {
            bresenhamPixels.RemoveAt(bresenhamPixels.Count - 1);
        }

        foreach (var bresenhamPixel in bresenhamPixels)
        {
            Vector3 newPixel = new Vector3(lastPixel.x + (bresenhamPixel.x * pixelScale), lastPixel.y + (bresenhamPixel.y * pixelScale));
            Instantiate(webPixel, newPixel, transform.rotation);
        }

        Vector3 lastBresenham = bresenhamPixels.Last();
        lastPixel = new Vector3(lastPixel.x + (lastBresenham.x * pixelScale), lastPixel.y + (lastBresenham.y * pixelScale));

        SetBounds();


    }

    void SetBounds()
    {
        outerTop = lastPixel.y + (pixelSize * pixelScale);
        outerBottom = lastPixel.y - (pixelSize * pixelScale);
        outerLeft = lastPixel.x - (pixelSize * pixelScale);
        outerRight = lastPixel.x + (pixelSize * pixelScale);

        innerTop = lastPixel.y + (pixelSize * (pixelScale / 2));
        innerBottom = lastPixel.y - (pixelSize * (pixelScale / 2));
        innerLeft = lastPixel.x - (pixelSize * (pixelScale / 2));
        innerRight = lastPixel.x + (pixelSize * (pixelScale / 2));
    }

    List<Vector2> BresenhamPixels(int x1, int y1)
    {

        List<Vector2> bresenhamPixels = new List<Vector2>();

        int x0 = 0;
        int y0 = 0;

        int dx = Math.Abs(x1);
        int dy = -Math.Abs(y1);

        int sx = x1 > 0 ? 1 : -1;
        int sy = y1 > 0 ? 1 : -1;

        int err = dx + dy;
        int e2;

        int failSafe = 0;

        while ((x0 != x1 | y0 != y1) & failSafe < 100)
        {
            failSafe += 1;

            e2 = 2 * err;

            if (e2 >= dy)
            {
                err += dy;
                x0 += sx;
            }
            if (e2 <= dx)
            {
                err += dx;
                y0 += sy;
            }
            bresenhamPixels.Add(new Vector2(x0, y0));
        }

        return bresenhamPixels;
    }

    bool IsColinear(Vector2 prevPoint, Vector2 currPoint, Vector2 nextPoint)
    {
        float minArea = 0.0000001f;
        float area = (( prevPoint.x * (currPoint.y - nextPoint.y) + currPoint.x * (nextPoint.y - prevPoint.y) + nextPoint.x * (prevPoint.y - currPoint.y) ) / 2);
        return Math.Abs(area) < minArea;
    }

    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2 [] v2 = new Vector2[v3.Length];
        for(int i = 0; i <  v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

    public bool ccw(Vector2 A, Vector2 B, Vector2 C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }

    public bool intersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        return ccw(A,C,D) != ccw(B,C,D) & ccw(A,B,C) != ccw(A,B,D);
    }

}