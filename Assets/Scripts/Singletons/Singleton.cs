using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
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
                    Debug.LogWarning("No instance of " + typeof(T).Name + " found in the scene. Creating a new one.");
                    var go = new GameObject(typeof(T).Name + "Auto-Generated");
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// Make sure to call base.Awake() in override if you need awake.
    /// </summary>
    protected virtual void Awake()
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
