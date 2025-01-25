using UnityEngine;

namespace Ky
{
    /// <summary>
    /// Exists all the time. 
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Parent<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static Transform I => instance?.transform;

        private static T instance;

        protected virtual void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (instance != null && instance != this)
            {
                Debug.LogWarning($"There is already an instance of type {typeof(T)}");
                Destroy(this);
            }
            else
            {
                instance = this as T;
            }
        }
    }
}