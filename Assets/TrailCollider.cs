using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Flat;
using Converter;
 
//Add this script to a new gameobject at 0,0,0
//make sure the gameobject position does not change.
public class TrailCollider : MonoBehaviour
{
    public TrailRenderer _tr; //assign the trailrenderer in editor.

    // Dylan Code
    Vector2 prevPoint;
    Vector2 currPoint;
    private int[] triangles;

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
        Vector3[] pointsInTrailRenderer3d = new Vector3[_tr.positionCount]; 
        _tr.GetPositions(pointsInTrailRenderer3d);

        Vector2[] pointsInTrailRenderer = pointsInTrailRenderer3d.toVector2Array(); 

        Vector2 lastPoint = pointsInTrailRenderer.Last();
        double lastX = Math.Round(lastPoint.x, 2);
        double lastY = Math.Round(lastPoint.y, 2);    

        if (lastX != currPoint.x | lastY != currPoint.y)
        {

            if (lastX != prevPoint.x | lastY != prevPoint.y)
            {
                if (pointsInTrailRenderer.Length > 1)
                {
                    Vector2[] pointsWithoutLast = pointsInTrailRenderer.SkipLast(1).ToArray();
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

 
                            // Filters out all points before the loop for calculating hitbox
                            var onlyLoop = pointsInTrailRenderer.Skip(i+1).ToArray();

                            bool meshTriangles = Triangulate(onlyLoop, out triangles, out string errorMessage);
                            // Debug.Log("Triangle Vertex Count: " + triangles.Length);
                            // Debug.Log("Vertex Count: " + onlyLoop.Length);

                            // Restart trail on loop
                            _tr.Clear();
                        }
                    }
                }
            }
            prevPoint = currPoint;
            currPoint = pointsInTrailRenderer.Last();
        }
    }

    public static Vector2[] FilterPoints(Vector2[] vertices) 
    {
        List<Vector2> finalVertices = new List<Vector2>();
        for (int i=0; i < vertices.Length; i++)
        {
            leftIndex = i - 1 < 0 ? vertices.Length - 1 : i - 1;
            rightIndex = i + 1 > vertices.Length - 1 ? 0 : i + 1;

            currentVertex = vertices[i];
            nextVertex = vertices[rightIndex];

            // Check if current point is identical to next
            if (currentVertex.x != nextVertex.x | currentVertex.y != nextVertex.y)
            {
                // Point is not identical to next
                // Check if point is on vector between previous and next vertex
                dxc = currPoint.x - point1.x;
                dyc = currPoint.y - point1.y;

                dxl = point2.x - point1.x;
                dyl = point2.y - point1.y;

                cross = dxc * dyl - dyc * dxl;

                if (cross == 0)
                {
                    if (abs(dxl) >= abs(dyl))
                    {
                        return dxl > 0 ? 
                        point1.x <= currPoint.x && currPoint.x <= point2.x :
                        point2.x <= currPoint.x && currPoint.x <= point1.x;
                    }
                    else
                    {
                        return dyl > 0 ? 
                        point1.y <= currPoint.y && currPoint.y <= point2.y :
                        point2.y <= currPoint.y && currPoint.y <= point1.y;
                    }
                }
                // Point is not between previous and next so we add to final vertices
                finalVertices.Add(currentVertex);
            }

            // float leftMiddleSide = Vector3.Distance(vertices[leftIndex], vertices[i]);
            // float rightMiddleSide = Vector3.Distance(vertices[rightIndex], vertices[i]);
            // float leftRightSide = Vector3.Distance(vertices[leftIndex], vertices[rightIndex]);

            // float pi = (float)Math.PI;
            // float angle = (180 / pi) * MathF.Acos(((leftMiddleSide*leftMiddleSide) + (rightMiddleSide*rightMiddleSide) - (leftRightSide*leftRightSide)) / (2 * leftMiddleSide * rightMiddleSide));

        }    
        return finalVertices.ToArray();
    }

    public static bool Triangulate(Vector2[] vertices, out int[] triangles, out string errorMessage)
    {
        triangles = null;
        errorMessage = string.Empty;

        if(vertices is null)
        {
            errorMessage = "The vertex list is null.";
            return false;
        }

        if(vertices.Length < 3)
        {
            errorMessage = "The vertex list must have at least 3 vertices.";
            return false;
        }

        if(vertices.Length > 1024)
        {
            errorMessage = "The max vertex list length is 1024";
            return false;
        }

        List<int> indexList = new List<int>();
        for(int i = 0; i < vertices.Length; i++)
        {
            indexList.Add(i);
        }

        int totalTriangleCount = vertices.Length - 2;
        int totalTriangleIndexCount = totalTriangleCount * 3;

        triangles = new int[totalTriangleIndexCount];
        int triangleIndexCount = 0;

        while(indexList.Count > 3)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int a = indexList[i];
                int b = Flat.Util.GetItem(indexList, i - 1);
                int c = Flat.Util.GetItem(indexList, i + 1);

                Vector2 va = vertices[a];
                Vector2 vb = vertices[b];
                Vector2 vc = vertices[c];

                Vector2 va_to_vb = vb - va;
                Vector2 va_to_vc = vc - va;

                // Is ear test vertex convex?
                if(Flat.Util.Cross(va_to_vb, va_to_vc) < 0f)
                {
                    continue;
                }

                bool isEar = true;

                // Does test ear contain any polygon vertices?
                for(int j = 0; j < vertices.Length; j++)
                {
                    if(j == a || j == b || j == c)
                    {
                        continue;
                    }

                    Vector2 p = vertices[j];

                    if(Flat.Util.IsPointInTriangle(p, vb, va, vc))
                    {
                        isEar = false;
                        break;
                    }                            
                }

                if(isEar)
                {
                    triangles[triangleIndexCount++] = b;
                    triangles[triangleIndexCount++] = a;
                    triangles[triangleIndexCount++] = c;

                    indexList.RemoveAt(i);
                    break;
                }
            }
        }

        triangles[triangleIndexCount++] = indexList[0];
        triangles[triangleIndexCount++] = indexList[1];
        triangles[triangleIndexCount++] = indexList[2];

        return true;
    }
}