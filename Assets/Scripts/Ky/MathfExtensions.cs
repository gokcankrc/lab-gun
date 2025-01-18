using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ky
{
    public static class MathfExtensions
    {
        public const float PI = Mathf.PI;
        public const float TAU = 2 * PI;
        public const float GOLDEN_RATIO = (1 + 2.2360679775f) / 2f;

        public static float Mod(this float a, float b)
        {
            float r = a % b;
            return r < 0 ? r + b : r;
        }

        public static float InverseLerpUnclamped(float a, float b, float value)
        {
            return ((value - a) / (b - a));
        }

        public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            value = InverseLerpUnclamped(fromMin, fromMax, value);
            return Mathf.LerpUnclamped(toMin, toMax, value);
        }

        /// <summary>
        /// Basically, shows up to "factor" significant digits.
        /// </summary>
        /// <param name="value">Value to round</param>
        /// <param name="significantDigits">Amount of significant digits that will stay</param>
        /// <returns></returns>
        public static float TrimInsignificantDigits(this float value, int significantDigits)
        {
            var sign = Mathf.Sign(value);
            var exponent = Mathf.Round(Mathf.Log10(Mathf.Abs(value)));
            double buffer = (double)Mathf.Abs(value) * (double)Mathf.Pow(10, -exponent + significantDigits);
            buffer = Math.Round(buffer);
            value = (float)(buffer * Math.Pow(10, exponent - significantDigits)) * sign;
            return value;
        }

        public static float RoundToMultiplesOfFactor(this float value, float factor)
        {
            return Mathf.Round(value / factor) * factor;
        }

        public static float RandomGaussian(float mean, float standardDeviation)
        {
            float u1 = 1f - Random.value;
            float u2 = 1f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
            return mean + standardDeviation * randStdNormal;
        }

        public static (int x, int y) GetClosestIntegerFactors(int n, float targetRatio, float errorCombineRatio = 0.5f)
        {
            float closestError = Mathf.Infinity;

            var x = 0;
            var y = 0;

            for (int i = 0; i <= Mathf.Sqrt(n); i++)
            {
                for (int j = n; j >= Mathf.Sqrt(n); j--)
                {
                    if (i * j < n) continue;
                    float currentRatio = i / (float)j;
                    float ratioError = Mathf.Abs(currentRatio - targetRatio) / targetRatio;
                    float countError = (i * j - n) / (float)n;
                    float combinedError = errorCombineRatio * ratioError + (1 - errorCombineRatio) * countError;

                    if (combinedError > closestError) continue;
                    closestError = combinedError;
                    x = i;
                    y = j;
                }
            }

            return (x, y);
        }

        public static Vector2 GetUnitVectorByAngle(float angleRad)
        {
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static Vector2 GetRandomUnitVector()
        {
            float randomAngle = Random.Range(0, TAU);
            return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        }

        public static Vector2 GetRandomPointInUnitCircle()
        {
            return Random.insideUnitCircle;
        }

        public static Vector3 GetRandomPointInUnitSphere()
        {
            return Random.insideUnitSphere;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radia"></param>
        /// <param name="angleRad"></param>
        /// <returns></returns>
        public static Vector2 GetPointOnEllipseWithAngle(Vector2 radia, float angleRad)
        {
            // 😎 maths time 😎
            // Ref: https://math.stackexchange.com/questions/22064/calculating-a-point-that-lies-on-an-ellipse-given-an-angle

            // Gizmos tester code cuz I didnt have a better place to store this amme hizmeti
            // private void OnDrawGizmos()
            // {
            // 	// tests the difference between parametric equations
            // 	var pos = PlayerGlobalVariables.PlayerWorldPos;
            //
            // 	var angle = anghtest;
            // 	var vector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            //
            // 	Vector3 a = MathfExtensions.GetPointOnEllipseWithAngleParametricEquation(new Vector2(2, 1),
            // 		Mathf.Deg2Rad * angle);
            // 	a = MathfExtensions.GetPointOnEllipseWithAngle(new Vector2(2, 1), Mathf.Deg2Rad * angle);
            // 	a = a;
            //
            // 	Gizmos.color = Color.green;
            // 	Gizmos.DrawLine(pos, pos + vector * 10);
            //
            // 	Gizmos.color = Color.blue;
            // 	Gizmos.DrawSphere(pos + a, 0.1f);
            // }

            var pi = Mathf.PI;
            var twopi = Mathf.PI * 2;

            angleRad = angleRad.Mod(twopi);

            var a = radia.x;
            var b = radia.y;
            var a2 = a * a;
            var b2 = b * b;

            var bottom = b2 + a2 * Mathf.Tan(angleRad) * Mathf.Tan(angleRad);
            var x = (a * b) / Mathf.Sqrt(bottom);

            var y2 = (1 - x * x / a2) * b2;
            var y = Mathf.Sqrt(y2);

            if ((pi / 2 < angleRad && angleRad < pi * 3 / 2))
                x = -x;
            if ((pi < angleRad && angleRad < 2 * pi))
                y = -y;

            return new Vector2(x, y);
        }

        /// <summary>
        /// The angle will not exactly match.
        /// </summary>
        public static Vector2 GetPointOnEllipseWithAngleParametricEquation(Vector2 radia, float angleRad)
        {
            return new Vector2(Mathf.Cos(angleRad) * radia.x, Mathf.Sin(angleRad) * radia.y);
        }

        public static Vector3[] GetDirectionsOnUnitSphere(int count, float percent)
        {
            if (count < 0) return Array.Empty<Vector3>();
            Vector3[] directionsOnUnitSphere = new Vector3[count];

            float angleIncrement = Mathf.PI * 2 * MathfExtensions.GOLDEN_RATIO;

            for (int i = 0; i < count; i++)
            {
                float t = (float)i / (count * (percent * 2 + 1));
                float inclination = Mathf.Acos(1 - 2 * t);
                float azimuth = angleIncrement * i;

                float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
                float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
                float z = Mathf.Cos(inclination);
                directionsOnUnitSphere[i] = new Vector3(x, y, z);
            }

            return directionsOnUnitSphere;
        }

        /// <summary>
        /// Returns corner positions of Cube
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector3[] CalculateCubeCorners(Vector3 center, Vector3 size, Vector3 offset = default)
        {
            // l => left  |  r => right
            // d => down  |  u => up
            // b => back  |  f => forward

            Vector3 ldb = center + offset - size * 0.5f;
            Vector3 ldf = center + offset - size.WithZ(-size.z) * 0.5f;
            Vector3 lub = center + offset - size.WithY(-size.y) * 0.5f;
            Vector3 rdb = center + offset - size.WithX(-size.x) * 0.5f;
            Vector3 luf = center + offset + size.WithX(-size.x) * 0.5f;
            Vector3 rdf = center + offset + size.WithY(-size.y) * 0.5f;
            Vector3 rub = center + offset + size.WithZ(-size.z) * 0.5f;
            Vector3 ruf = center + offset + size * 0.5f;

            return new[]
            {
                ldb,
                ldf,
                lub,
                rdb,
                luf,
                rdf,
                rub,
                ruf
            };
        }
    }
}