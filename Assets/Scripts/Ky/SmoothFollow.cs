using UnityEngine;

namespace Ky
{
    public class SmoothFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        [SerializeField] private float cameraFollowSpeed;

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * cameraFollowSpeed);
        }
    }
}