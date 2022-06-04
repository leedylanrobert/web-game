using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
 
//Add this script to a new gameobject at 0,0,0
//make sure the gameobject position does not change.
public class TrailCollider : MonoBehaviour
{
    public TrailRenderer _tr; //assign the trailrenderer in editor.

    // Dylan Code
    Vector3 prevPoint;
    Vector3 currPoint;

    bool crossed = false;

    void Awake()
    {
    }
    void Update()
    {
        UpdateCollider();
    }
    void UpdateCollider()
    {
        Vector3[] pointsInTrailRenderer = new Vector3[_tr.positionCount]; 
        _tr.GetPositions(pointsInTrailRenderer);

        Vector3 lastPoint = pointsInTrailRenderer.Last();
        double lastX = Math.Round(lastPoint.x, 2);
        double lastY = Math.Round(lastPoint.y, 2);    

        if (lastX != currPoint.x | lastY != currPoint.y)
        {

            if (lastX != prevPoint.x | lastY != prevPoint.y)
            {
                if (pointsInTrailRenderer.Length > 1)
                {
                    Vector3[] pointsWithoutLast = pointsInTrailRenderer.SkipLast(1).ToArray();
                    double[,] positions = new double[pointsWithoutLast.Length, 2];
                    for(int i=0; i < pointsWithoutLast.Length; i++)
                    {
                        double positionX = Math.Round(pointsWithoutLast[i].x, 2);
                        double positionY = Math.Round(pointsWithoutLast[i].y, 2);
                        positions[i,0] = positionX;
                        positions[i,1] = positionY;
                    }

                    for(int i=0; i < positions.GetLength(0); i++)
                    {
                        double errorMargin = 0.07;
                        double xDiff = Math.Abs(lastX - positions[i,0]);
                        double yDiff = Math.Abs(lastY - positions[i,1]);

                        if (xDiff < errorMargin & yDiff < errorMargin)
                        {
                            Debug.Log("CROSSED!");
                            crossed = true;
                            Debug.Log("Vertices Count: " + positions.GetLength(0));

                            // Filters out all points before the loop for calculating hitbox
                            var onlyLoop = pointsInTrailRenderer.Skip(i+1).ToArray();
                            Utils.DLL everyPoint =  ClipEars(onlyLoop);
                            Debug.Log("Ear clips head prev: " + everyPoint.head.prev.prev.data);
                            Debug.Log("Ear clips head next: " + everyPoint.head.prev.next.data);
                            Vector3[] freshList = new Vector3[]{};
                            _tr.Clear();
                        }
                    }
                }
            }
            prevPoint = currPoint;
            currPoint = pointsInTrailRenderer.Last();
        }

        List<Vector2> edgePoints = new List<Vector2>();
        foreach(Vector3 point in pointsInTrailRenderer)
        {
            edgePoints.Add(new Vector2(point.x,point.y));
        }
        // string[] resultArray = Array.ConvertAll(pointsInTrailRenderer, x => x.ToString());
        if (!crossed)
        {
            Debug.Log("$$$$$$ Prev Point $$$$$$: " + prevPoint);
            Debug.Log("$$$$$$ Curr Point $$$$$$: " + currPoint);
        }
    }

    public static Utils.DLL ClipEars(Vector3[] polygonPoints)
    {
        double xMin = Math.Round(polygonPoints[0].x, 2);
        int minNodeIndex = 0;
        // Vector3 minVector = polygonPoints.First(point => point.x == xMin);

        Utils.DLL.Node firstNode = new Utils.DLL.Node(0);
        Utils.DLL allPoints = new Utils.DLL();
        Utils.DLL.Node leftNode = firstNode;
        allPoints.head = firstNode; 


        Debug.Log("Polygonpoints length: " + polygonPoints.Length);

        // Get index of the leftmost node which is certainly a convex node
        for (int i=1; i < polygonPoints.Length; i++)
        {    
            Utils.DLL.Node nextNode = new Utils.DLL.Node(i);    
            firstNode.next = nextNode;
            nextNode.prev = firstNode;
            firstNode = nextNode;
            if (polygonPoints[i].x < xMin)
            {
                leftNode = firstNode;
                minNodeIndex = i;
            }
        }
        Debug.Log("Leftmost node index: " + minNodeIndex);

        Utils.DLL convexPoints = new Utils.DLL();
        Utils.DLL reflexPoints = new Utils.DLL();

        // Utils.DLL.Node leftNode;
        // Utils.DLL.Node middleNode;

        for (int i=0; i < polygonPoints.Length; i++)
        {    
            int rightIndex = minNodeIndex + i;
            int middleIndex = rightIndex - 1;
            int leftIndex = rightIndex - 2;

            Utils.DLL.Node rightNode = new Utils.DLL.Node(rightIndex);

            if (i == 0)
            {
                convexPoints.head = rightNode;
            }
            else if (i == 1)
            {}
            else
            {
                float leftMiddleSide = Vector3.Distance(polygonPoints[leftIndex], polygonPoints[middleIndex]);
                float rightMiddleSide = Vector3.Distance(polygonPoints[rightIndex], polygonPoints[middleIndex]);
                float leftRightSide = Vector3.Distance(polygonPoints[leftIndex], polygonPoints[rightIndex]);

                float angle = MathF.Acos(((leftMiddleSide*leftMiddleSide) + (rightMiddleSide*rightMiddleSide) - (leftRightSide*leftRightSide)) / (2 * leftMiddleSide * rightMiddleSide));
            }
        }

        firstNode.next = allPoints.head;
        allPoints.head.prev = firstNode;

        return allPoints;
    }
}