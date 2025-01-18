using System.Collections.Generic;
using UnityEngine;

namespace Ky
{
    public static class Vector2Extensions
    {
        public static Vector3 ToXY(this Vector2 v2)
        {
            return v2; // lul
        }

        public static Vector3 ToXZ(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

        public static Vector3 ToYZ(this Vector2 v2)
        {
            return new Vector3(0, v2.x, v2.y);
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector2 Rotate90AroundZ(this Vector2 v2)
        {
            return new Vector2(-v2.y, v2.x);
        }

        /// <returns>Returns the signed angle in degrees between Vector2.Right, aka. default geometry angle.</returns>
        public static float Angle(this Vector2 v2)
        {
            return Vector2.SignedAngle(Vector2.right, v2);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        /// <summary>
        /// Returns the closest Point to point in the given IEnumerable, or itself,
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Vector2 FindClosest(this Vector2 point, IEnumerable<Vector2> points, out int index)
        {
            float closetDistance = Mathf.Infinity;
            Vector2 closest = point;
            index = -1;
            var i = 0;

            foreach (Vector2 other in points)
            {
                float distance = (point - other).sqrMagnitude;
                if (distance < closetDistance)
                {
                    closetDistance = distance;
                    closest = other;
                    index = i;
                }

                i++;
            }

            return closest;
        }

        /// <summary>
        /// Returns the furthest Point to point in the given IEnumerable, or itself,
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Vector2 FindFurthest(this Vector2 point, IEnumerable<Vector2> points, out int index)
        {
            float furthestDistance = 0;
            Vector2 furthest = point;
            index = -1;
            var i = 0;

            foreach (Vector2 other in points)
            {
                float distance = (point - other).sqrMagnitude;
                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthest = other;
                    index = i;
                }

                i++;
            }

            return furthest;
        }

        public static float GetRandomBetween(this Vector2 v)
        {
            return Random.Range(v.x, v.y);
        }

        public static int GetRandomBetween(this Vector2Int v)
        {
            return Random.Range(v.x, v.y);
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max vectors
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector2 ClampElements(this Vector2 v, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max values
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector2 ClampElements(this Vector2 v, float min, float max)
        {
            return new Vector2(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max vectors
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector2Int ClampElements(this Vector2Int v, Vector2Int min, Vector2Int max)
        {
            return new Vector2Int(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max values
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector2Int ClampElements(this Vector2Int v, int min, int max)
        {
            return new Vector2Int(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
        }
    }
}