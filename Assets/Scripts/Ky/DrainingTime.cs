using System;
using UnityEngine;

namespace Ky
{
    [Serializable]
    public class DrainingTime
    {
        public Action onJustDrained;

        public float max;
        public bool loop;

        public float Ratio => Current / max;
        public float RatioTillDrain => 1 - Current / max;

        public float Current { get; set; }
        public bool JustDrained { get; set; }
        public bool IsDrained { get; set; }

        public DrainingTime(float maxAndCurrent)
        {
            max = maxAndCurrent;
            Current = maxAndCurrent;

            onJustDrained = null;
            loop = false;
            JustDrained = false;
            IsDrained = false;
        }

        public DrainingTime(float max, float current = 0, bool loop = false)
        {
            this.max = max;
            this.loop = loop;
            Current = current;
            if (loop == true && current == 0)
                current = max;

            onJustDrained = null;
            JustDrained = false;
            IsDrained = false;
        }

        public void Update(float deltaTime)
        {
            bool isWayTooBelowZero = Current < 0;
            Current -= deltaTime;

            JustDrained = false;

            if (IsDrained == false && Current <= 0)
            {
                JustDrained = true;
                if (loop)
                {
                    if (isWayTooBelowZero)
                        SetToMax();
                    else
                        SetToMaxAdditive();
                }

                onJustDrained?.Invoke();
            }

            IsDrained = (Current <= 0);
        }

        public void SetToMax()
        {
            Current = max;
        }

        public void SetToMaxAdditive()
        {
            // For more consistently-timed loops
            Current += max;
        }

        public void ClampedChange(float change)
        {
            Current = Mathf.Clamp(Current + change, 0, max);
        }

        public void Clamp()
        {
            Current = Mathf.Clamp(Current, 0, max);
        }
    }
}