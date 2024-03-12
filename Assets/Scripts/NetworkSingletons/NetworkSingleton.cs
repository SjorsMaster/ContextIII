using Mirror;
using UnityEngine;

namespace ContextIII.NetworkSingletons
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkSingleton<T> : NetworkBehaviour where T : Component
    {
        protected static T instance;

        public static bool HasInstance => instance != null;
        public static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + "Auto-Generated");
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }
        
        /// <summary>
        /// Make sure to call base.Start() in override if you need Start.
        /// </summary>
        protected virtual void Start()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
                return;

            instance = this as T;
        }
    }
}
