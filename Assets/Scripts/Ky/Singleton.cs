using UnityEngine;

namespace Ky
{
    /// <summary>
    /// Exists all the time. 
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T i;

        public static T I
        {
            get
            {
#if UNITY_EDITOR
                if (i == null)
                    if (Application.isPlaying == false)
                        i = (T)FindObjectOfType(typeof(T));
#endif
                return i;
            }
            protected set { i = value; }
        }

        protected virtual void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (I != null && I != this)
            {
                Debug.LogWarning($"There is already an instance of type {typeof(T)}");
                Destroy(this);
            }
            else
            {
                I = this as T;
            }
        }
    }
}