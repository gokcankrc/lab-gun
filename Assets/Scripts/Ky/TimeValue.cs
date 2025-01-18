using UnityEngine;

namespace Ky
{
    public struct TimeValue
    {
        public float value;

        public int Minutes => Mathf.FloorToInt(value / 60);
        public int Seconds => Mathf.FloorToInt(value.Mod(60));

        public TimeValue(float value)
        {
            this.value = value;
        }

        public string GetCurrentTime()
        {
            return string.Format("{0:00}:{1:00}", Minutes, Seconds);
        }
    }
}