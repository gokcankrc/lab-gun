using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Ky
{
    public class SinusMovement : MonoBehaviour
    {
        [SerializeField] private Axis axis = Axis.X;
        public bool isActive = true;
        public bool worldCoordinates = false;
        private float oscilationTime = 1;
        [SerializeField] private float length = 1;
        [SerializeField] private bool unscaled = false;
        [SerializeField] private bool randomStart = true;
        [SerializeField, HideIf("randomStart"), Range(0f, 360f)] private float phase;
        private Vector3 originalLocalPosition;

        private Vector3 GetOffsetDirection
        {
            get
            {
                if (worldCoordinates)
                {
                    return axis switch
                    {
                        Axis.X => Vector3.right,
                        Axis.Y => Vector3.up,
                        Axis.Z => Vector3.forward,
                        _ => throw new System.ArgumentOutOfRangeException(),
                    };
                }
                else
                {
                    return axis switch
                    {
                        Axis.X => transform.right,
                        Axis.Y => transform.up,
                        Axis.Z => transform.forward,
                        _ => throw new System.ArgumentOutOfRangeException(),
                    };
                }
            }
        }

        private void Start()
        {
            originalLocalPosition = transform.localPosition;
            phase = randomStart ? Random.Range(0, 360f) : 0;
        }

        private void Update()
        {
            if (!isActive) return;

            var currentTime = unscaled ? Time.unscaledTime : Time.time;
            transform.localPosition = originalLocalPosition;
            transform.position += GetOffsetVector(currentTime);
        }

        Vector3 GetOffsetVector(float time)
        {
            return GetOffsetDirection * (GetOscilationValue(time) * length) / 2;
        }

        private float GetOscilationValue(float time)
        {
            return Mathf.Sin(MathfExtensions.TAU / this.oscilationTime * time + phase);
        }

        [Button]
        private void SetAxisBecauseOdinIsFucked(int index)
        {
            axis = (Axis)index;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != gameObject) return;

            Gizmos.color = Color.cyan;
            var position = transform.position;
            float time = 0;
            if (Application.isPlaying)
                time = unscaled ? Time.unscaledTime : Time.time;

            var offsetUp = GetOffsetDirection * (1 - GetOscilationValue(time)) * length / 2;
            var offsetDown = GetOffsetDirection * (-1 - GetOscilationValue(time)) * length / 2;
            Gizmos.DrawWireSphere(position + offsetUp, 0.1f);
            Gizmos.DrawWireSphere(position + offsetDown, 0.1f);
        }
#endif

        private enum Axis
        {
            X,
            Y,
            Z
        }
    }
}