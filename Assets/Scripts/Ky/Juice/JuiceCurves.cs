using UnityEngine;

namespace Ky.Juice
{
    [CreateAssetMenu(menuName = "Ky/Juice Curves", fileName = "Juice Curves")]
    public class JuiceCurves : ScriptableObject
    {
        [Space] public bool effectsPosition = false;
        public AnimationCurve xPosCurve = AnimationCurve.Linear(0, 0, 1, 0);
        public AnimationCurve yPosCurve = AnimationCurve.Linear(0, 0, 1, 0);
        public AnimationCurve zPosCurve = AnimationCurve.Linear(0, 0, 1, 0);

        [Space] public bool effectsScale = false;
        public AnimationCurve xScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve yScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve zScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Space] public bool effectsRotation = false;
        public AnimationCurve xRotationCurve = AnimationCurve.Linear(0, 0, 1, 0);
        public AnimationCurve yRotationCurve = AnimationCurve.Linear(0, 0, 1, 0);
        public AnimationCurve zRotationCurve = AnimationCurve.Linear(0, 0, 1, 0);
    }
}