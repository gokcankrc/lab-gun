using UnityEngine;

namespace Ky
{
    public class RotateContinuously : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationSpeed;
        [SerializeField] private Vector3 rotationAxes;

        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;

        private void FixedUpdate()
        {
            var objTransform = transform;
            var currentRotation = objTransform.localRotation;
            var eulerAngles = currentRotation.eulerAngles;

            var rotationAngles = new float[3];
            for (var i = 0; i < 3; i++)
            {
                rotationAngles[i] = eulerAngles[i] + (rotationAxes[i] * rotationSpeed[i]);
            }

            objTransform.localRotation = Quaternion.Euler(rotationAngles[X], rotationAngles[Y], rotationAngles[Z]);
        }
    }
}