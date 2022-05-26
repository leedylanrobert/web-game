using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(EdgeCollider2D))]
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
                            crossed = true;

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
}