using UnityEngine;

namespace Ky.Popup
{
    public class PopupParent : MonoBehaviour
    {
        public Transform i;

        private void Awake()
        {
            i = transform;
        }
    }
}