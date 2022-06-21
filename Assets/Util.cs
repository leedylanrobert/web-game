using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flat
{
    public static class Util
    { 
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y - b.x;
        }
        public static T GetItem<T>(T[] array, int index)
        {
            if(index >= array.Length)
            {
                return array[index % array.Length];
            }
            else if (index < 0)
            {
                return array[index % array.Length + array.Length];
            }
            else
            {
                return array[index];
            }
        }

        public static T GetItem<T>(List<T> array, int index)
        {
            if(index >= array.Count)
            {
                return array[index % array.Count];
            }
            else if (index < 0)
            {
                return array[index % array.Count + array.Count];
            }
            else
            {
                return array[index];
            }
        }

        public static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - a;
            Vector2 ca = a - c;

            Vector2 ap = p - a;
            Vector2 bp = p - b;
            Vector2 cp = p - c;

            float cross1 = Util.Cross(ab, ap);
            float cross2 = Util.Cross(bc, bp);
            float cross3 = Util.Cross(ca, cp);

            if(cross1 > 0f || cross2 > 0f || cross3 > 0f)
            {
                return false;
            }
            return true;
        }
    }
}