using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

namespace Ky
{
    public static class Vector3Extensions
    {
        public static Vector2 FromXY(this Vector3 v3)
        {
            return v3;
        }

        public static Vector2 FromXZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }

        public static Vector2 FromYZ(this Vector3 v3)
        {
            return new Vector2(v3.y, v3.z);
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 ElementwiseMultiplication(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Returns the closest Point to point in the list, or itself,
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Vector3 FindClosest(this Vector3 point, Vector3[] points, out int index)
        {
            float closetDistance = Mathf.Infinity;
            Vector3 closest = point;
            index = -1;

            for (var i = 0; i < points.Length; i++)
            {
                float distance = (point - points[i]).sqrMagnitude;
                if (distance < closetDistance)
                {
                    closetDistance = distance;
                    closest = points[i];
                    index = i;
                }
            }

            return closest;
        }

        /// <summary>
        /// Returns indices of the closest point to Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Vector3Int FindClosest(this Vector3 point, Vector3[,,] points, out float distance)
        {
            float closetDistance = Mathf.Infinity;
            var coord = new Vector3Int(-1, -1, -1);

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    for (int k = 0; k < points.GetLength(2); k++)
                    {
                        float dist = (points[i, j, k] - point).sqrMagnitude;
                        if (dist < closetDistance)
                        {
                            closetDistance = dist;
                            coord = new Vector3Int(i, j, k);
                        }
                    }
                }
            }

            distance = Mathf.Sqrt(closetDistance);
            return coord;
        }

        /// <summary>
        /// Component-wise absolute value
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        /// <summary>
        /// Component-wise min value
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 MaxComponent(Vector3 a, Vector3 b)
        {
            return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
        }

        public static Vector3 MinComponent(Vector3 a, Vector3 b)
        {
            return new Vector3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
        }

        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }

        public static float InverseLerpUnclamped(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            return Mathf.Clamp(InverseLerpUnclamped(a, b, value), 0, 1);
        }

        public static float DistanceToLine(Vector3 point, Vector3 linePoint, Vector3 lineDirection)
        {
            return Vector3.Cross((linePoint - point), lineDirection.normalized).magnitude;
        }

        public static float SignedDistanceToLine(Vector3 point, Vector3 linePoint, Vector3 lineDirection, Vector3 up)
        {
            Vector3 cross = Vector3.Cross((linePoint - point), lineDirection.normalized);
            float dot = Vector3.Dot(cross, up);
            return Mathf.Sign(dot) * cross.magnitude;
        }

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            float d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine,
            bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            float d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineDirection1,
            Vector3 linePoint2, Vector3 lineDirection2)
        {
            Vector3 lineDirection3 = linePoint2 - linePoint1;
            Vector3 crossVec1And2 = Vector3.Cross(lineDirection1, lineDirection2);
            Vector3 crossVec3And2 = Vector3.Cross(lineDirection3, lineDirection2);

            float planarFactor = Vector3.Dot(lineDirection3, crossVec1And2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1And2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3And2, crossVec1And2) / crossVec1And2.sqrMagnitude;
                intersection = linePoint1 + (lineDirection1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }

        public static float CalculateDistanceBetweenPoints(Vector3[] points)
        {
            var distance = 0f;

            for (int i = 1; i < points.Length; i++)
            {
                distance += Vector3.Distance(points[i], points[i - 1]);
            }

            return distance;
        }

        private static Vector3 GetRandomVectorInBetween(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Random.Range(v1.x, v2.x), Random.Range(v1.y, v2.y), Random.Range(v1.z, v2.z));
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max vectors
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 ClampElements(this Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z));
        }

        /// <summary>
        /// Clamp the elements of the vector in between respective elements of min and max value
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 ClampElements(this Vector3 v, float min, float max)
        {
            return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
        }

        /// <summary>
        /// Returns count closest positions to position
        /// </summary>
        /// <param name="positions">Positions to compare to position</param>
        /// <param name="position">Origin point to to compare others</param>
        /// <param name="count">How many positions to return</param>
        /// <returns></returns>
        [Pure]
        public static Vector3[] GetClosestPositions(this IEnumerable<Vector3> positions, Vector3 position, int count)
        {
            if (count >= positions.Count()) return positions.ToArray();

            var closestEntities = new Vector3[count];
            var closestDistances = new float[count];

            var currentCount = 0;

            for (var i = 0; i < closestDistances.Length; i++)
            {
                closestDistances[i] = Mathf.Infinity;
            }

            foreach (Vector3 v3 in positions)
            {
                float dist = (v3 - position).sqrMagnitude;
                if (closestDistances[currentCount] > dist)
                    Add(dist, v3);
            }

            return closestEntities;

            void Add(float dist, Vector3 v3)
            {
                for (int i = closestDistances.Length - 1; i >= -1; i--)
                {
                    if (i == -1)
                    {
                        MoveUp(1); // Can't set 0 to -1, so we give 1 instead of 0 to the method
                        closestDistances[0] = dist;
                        closestEntities[0] = v3;
                        break;
                    }


                    if (closestDistances[i] > dist) continue;

                    MoveUp(i + 1);
                    closestDistances[i + 1] = dist;
                    closestEntities[i + 1] = v3;
                    break;
                }

                currentCount = Mathf.Min(currentCount + 1, count - 1);
            }

            void MoveUp(int from)
            {
                for (int j = closestDistances.Length - 1; j >= from; j--)
                {
                    closestDistances[j] = closestDistances[j - 1];
                    closestEntities[j] = closestEntities[j - 1];
                }
            }
        }

        /// <summary>
        /// Returns count closest positions to position
        /// </summary>
        /// <param name="positions">Positions to compare to position</param>
        /// <param name="position">Origin point to to compare others</param>
        /// <param name="count">How many positions to return</param>
        /// <param name="indices">Index of the positions</param>
        /// <returns></returns>
        [Pure]
        public static Vector3[] GetClosestPositions(this IEnumerable<Vector3> positions, Vector3 position, int count, out int[] indices)
        {
            IEnumerable<Vector3> enumerable = positions as Vector3[] ?? positions.ToArray();
            var positionsArray = enumerable.ToArray();


            if (count >= positionsArray.Length)
            {
                indices = new int[positionsArray.Length];
                for (int i = 0; i < positionsArray.Length; i++)
                {
                    indices[i] = i;
                }

                return positionsArray;
            }

            var closestEntities = new Vector3[count];
            var closestDistances = new float[count];
            indices = new int[count];

            var currentCount = 0;

            for (var i = 0; i < closestDistances.Length; i++)
            {
                closestDistances[i] = Mathf.Infinity;
            }

            for (int i = 0; i < positionsArray.Length; i++)
            {
                float dist = (positionsArray[i] - position).sqrMagnitude;
                if (closestDistances[currentCount] > dist)
                    Add(dist, positionsArray[i], i, ref indices);
            }


            return closestEntities;

            void Add(float dist, Vector3 v3, int index, ref int[] indices)
            {
                for (int i = closestDistances.Length - 1; i >= -1; i--)
                {
                    if (i == -1)
                    {
                        MoveUp(1, ref indices); // Can't set 0 to -1, so we give 1 instead of 0 to the method
                        closestDistances[0] = dist;
                        closestEntities[0] = v3;
                        indices[0] = index;
                        break;
                    }


                    if (closestDistances[i] > dist) continue;

                    MoveUp(i + 1, ref indices);
                    closestDistances[i + 1] = dist;
                    closestEntities[i + 1] = v3;
                    indices[i + 1] = index;
                    break;
                }

                currentCount = Mathf.Min(currentCount + 1, count - 1);
            }

            void MoveUp(int from, ref int[] indices)
            {
                for (int j = closestDistances.Length - 1; j >= from; j--)
                {
                    closestDistances[j] = closestDistances[j - 1];
                    closestEntities[j] = closestEntities[j - 1];
                    indices[j] = indices[j - 1];
                }
            }
        }
    }
}