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

        Vector2[] pointsInTrailRenderer = ConvertArray(pointsInTrailRenderer3d); 

        Debug.Log("Check 1");
        Vector2 newPoint = pointsInTrailRenderer.Last();
        double newX = newPoint.x;
        double newY = newPoint.y;

        if (pointsInTrailRenderer.Length > 1)
        {
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
                            Debug.Log("New Point:" + newPoint.x + ", " + newPoint.y);
                            Debug.Log("Prev Point: " + prevPoint.x + ", " + prevPoint.y);
                            Debug.Log("Head Point: " + headPoint.x + ", " + headPoint.y);
                            Debug.Log("Tail Point: " + tailPoint.x + ", " + tailPoint.y);
                        }
                    }
                }
            }
        }

        
        if (crossed)
        {
            Debug.Log("CROSS!!!!!!!!!!!!!!!!");
        }
    }

     Vector2[] ConvertArray(Vector3[] v3){
     Vector2 [] v2 = new Vector2[v3.Length];
     for(int i = 0; i <  v3.Length; i++){
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

    // public static Vector2[] FilterPoints(Vector2[] vertices) 
    // {
    //     List<Vector2> finalVertices = new List<Vector2>();
    //     for (int i=0; i < vertices.Length; i++)
    //     {
    //         int leftIndex = i - 1 < 0 ? vertices.Length - 1 : i - 1;
    //         int rightIndex = i + 1 > vertices.Length - 1 ? 0 : i + 1;

    //         Vector2 prevVertex = vertices[leftIndex];
    //         Vector2 currentVertex = vertices[i];
    //         Vector2 nextVertex = vertices[rightIndex];

    //         // Check if current point is identical to next
    //         if (currentVertex.x != nextVertex.x | currentVertex.y != nextVertex.y)
    //         {
    //             // Point is not identical to next
    //             // Check if point is on vector between previous and next vertex
    //             float dxc = currentVertex.x - prevVertex.x;
    //             float dyc = currentVertex.y - prevVertex.y;

    //             float dxl = nextVertex.x - prevVertex.x;
    //             float dyl = nextVertex.y - prevVertex.y;

    //             float cross = dxc * dyl - dyc * dxl;

    //             if (cross == 0)
    //             //point is on the line, check if it's between two vertices
    //             {
    //                 if (Math.Abs(dxl) >= Math.Abs(dyl))
    //                 {
    //                     return dxl > 0 ? 
    //                     prevVertex.x <= currentVertex.x && currentVertex.x <= nextVertex.x :
    //                     nextVertex.x <= currentVertex.x && currentVertex.x <= prevVertex.x;
    //                 }
    //                 else
    //                 {
    //                     return dyl > 0 ? 
    //                     prevVertex.y <= currentVertex.y && currentVertex.y <= nextVertex.y :
    //                     nextVertex.y <= currentVertex.y && currentVertex.y <= prevVertex.y;
    //                 }
    //             }
    //             // Point is not between previous and next so we add to final vertices
    //             finalVertices.Add(currentVertex);
    //         }

    //         // float leftMiddleSide = Vector3.Distance(vertices[leftIndex], vertices[i]);
    //         // float rightMiddleSide = Vector3.Distance(vertices[rightIndex], vertices[i]);
    //         // float leftRightSide = Vector3.Distance(vertices[leftIndex], vertices[rightIndex]);

    //         // float pi = (float)Math.PI;
    //         // float angle = (180 / pi) * MathF.Acos(((leftMiddleSide*leftMiddleSide) + (rightMiddleSide*rightMiddleSide) - (leftRightSide*leftRightSide)) / (2 * leftMiddleSide * rightMiddleSide));

    //     }    
    //     return finalVertices.ToArray();
    // }

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