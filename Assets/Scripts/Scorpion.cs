using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Scorpion : MonoBehaviour
{

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    Vector2 targetPosition;
    Vector2 startingPosition;

    public float minSpeed;
    public float maxSpeed;

    public float secondsToMaxDifficulty;

    public Entry entry;
    private bool startMoving = false;

    bool targeted = false;

    // New
    float distanceToEndPoint;
    float acceleration = .0001f;
    float timeCount;
    float slowingDistance = -1f;
    Vector2 halfwayPoint;
    float initialVelocity = 0f;
    float deceleration;
    float halfwayVelocity;
    float lastDistance = 0f;

    Vector2 lastPosition;

    public TrailCollider trailCollider;

    public AudioClip snip;
    float volume = 0.5f;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        trailCollider = GameObject.Find("Spider").GetComponent<TrailCollider>();
        startingPosition = transform.position;
        lastPosition = startingPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if (startMoving)
        {
            float xDiff;
            float yDiff;

            if (!targeted)
            {
                targetPosition = GameObject.Find("Spider").transform.position;
                // Choose position 8f+ in the direction of spider
                xDiff = targetPosition.x - transform.position.x;
                yDiff = targetPosition.y - transform.position.y;


                while (Mathf.Sqrt((xDiff * xDiff) + (yDiff * yDiff)) < 6f)
                {
                    targetPosition.x += xDiff;
                    targetPosition.y += yDiff;
                    xDiff = targetPosition.x - transform.position.x;
                    yDiff = targetPosition.y - transform.position.y;
                }
                targetPosition = Vector2.MoveTowards(transform.position, targetPosition, 6f);

                startingPosition = transform.position;
                targeted = true;
            }

            xDiff = targetPosition.x - transform.position.x;
            yDiff = targetPosition.y - transform.position.y;

            distanceToEndPoint = (Mathf.Sqrt((xDiff * xDiff) + (yDiff * yDiff)));
            
            if (slowingDistance < 0)
            {
                timeCount = 0f;
                slowingDistance = 3f;
                acceleration = slowingDistance / .75f;

                halfwayPoint = Vector2.MoveTowards(transform.position, targetPosition, 3f);
                halfwayVelocity = acceleration * .75f;

            }
            
            timeCount += Time.deltaTime;

            float distance;

            distance = (initialVelocity * timeCount) + acceleration * (timeCount * timeCount) / 2;

            if (distance > slowingDistance)
            {
                if (initialVelocity == 0f)
                {
                    timeCount = 0f;
                    startingPosition = halfwayPoint;
                    initialVelocity = halfwayVelocity;
                    acceleration = -acceleration;
                    lastDistance = 0f;
                }
            }

            distance = (initialVelocity * timeCount) + acceleration * (timeCount * timeCount) / 2;

            if (distance < lastDistance)
            // Scorpion has reached its max distance, time to retarget
            {
                targeted = false;
                slowingDistance = -1f;
                initialVelocity = 0f;
                lastDistance = 0f;

            }
            else
            {
                lastPosition = transform.position;
                transform.position = startingPosition;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance);

                Vector2 currPosition = new Vector2(transform.position.x, transform.position.y);

                // Check here if there was an intersection
                isCrossed(lastPosition, currPosition);
                lastDistance = distance;
            }


        }
        else if (entry.isSpawned == true) 
        {
            startMoving = true;
        }
    }

    public bool ccw(Vector2 A, Vector2 B, Vector2 C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }

    public bool intersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        return ccw(A,C,D) != ccw(B,C,D) & ccw(A,B,C) != ccw(A,B,D);
    }

    void isCrossed(Vector2 lastPosition, Vector2 currPosition)
    {
        // Check if each of the previous segments intersect with current segment
        Vector2[] trailPoints = trailCollider.trailPoints;
        if (trailPoints.Length > 1)
        {
            bool crossed = false;
            Vector2 tailPoint = trailPoints[0];
            Vector2 headPoint = trailPoints[0];
            for (int i=0; i < trailPoints.Length; i++)
            {
                tailPoint = headPoint;
                headPoint = trailPoints[i];

                // Don't check segment if head and tail point are identical
                // Also don't check segment if head and prev point are identical
                if ((headPoint.x != tailPoint.x | headPoint.y != tailPoint.y) & (headPoint.x != lastPosition.x | headPoint.y != lastPosition.y))
                {
                    crossed = intersectScorp(currPosition, lastPosition, headPoint, tailPoint);
                    if (crossed)
                    {

                        Vector2 intersection = CreateIntersection(currPosition, lastPosition, headPoint, tailPoint);

                        Vector2[] remainder = trailPoints.Skip(i).ToArray();

                        Vector2[] front = new Vector2[]{intersection};
                        Vector2[] combined = front.Concat(remainder).ToArray();
                        Vector3[] final = new Vector3[remainder.Length];
                        for (int j = 0; j < remainder.Length; j++)
                        {
                            final[j] = new Vector3(remainder[j].x, remainder[j].y, 0f);
                        }

                        trailCollider._tr.Clear();
                        trailCollider._tr.AddPositions(final);
                    }
                }
            }
        }
    }

    public bool intersectScorp(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        return ccwScorp(A,C,D) != ccwScorp(B,C,D) & ccwScorp(A,B,C) != ccwScorp(A,B,D);
    }

    public bool ccwScorp(Vector2 A, Vector2 B, Vector2 C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
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
}
