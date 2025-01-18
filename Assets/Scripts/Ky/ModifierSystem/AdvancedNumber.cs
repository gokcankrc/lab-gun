using System;

namespace Ky.ModifierSystem
{
    [Serializable]
    public struct AdvancedNumber
    {
        public float baseValue;
        public float additiveMult;
        // Make buckets, somehow.
        public float multiplicativeMult;

        public AdvancedNumber(float baseVal, float additiveMult = 0, float addition = 0,
            float multiplicativeMult = 1f)
        {
            baseValue = baseVal;
            this.additiveMult = additiveMult;
            this.multiplicativeMult = multiplicativeMult;
        }

        public static AdvancedNumber operator +(AdvancedNumber a, AdvancedNumber b)
        {
            return new AdvancedNumber(
                a.baseValue + b.baseValue,
                a.additiveMult + b.additiveMult,
                a.multiplicativeMult * b.multiplicativeMult
            );
        }

        public static AdvancedNumber operator -(AdvancedNumber a, AdvancedNumber b)
        {
            return new AdvancedNumber(
                a.baseValue - b.baseValue,
                a.additiveMult - b.additiveMult,
                a.multiplicativeMult / b.multiplicativeMult
            );
        }

        public float Calculate()
        {
            return CalculateFrom(baseValue, additiveMult, multiplicativeMult);
        }

        public static float CalculateFrom(float baseVal, float additiveMult, float multMult)
        {
            float finalVal = baseVal * (1 + additiveMult) * multMult;
            return finalVal;
        }
    }
}