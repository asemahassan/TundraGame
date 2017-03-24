using UnityEngine;
using System.Collections;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T instance;
    private static object _syncRoot;
    private static object syncRoot { get { if (_syncRoot == null) _syncRoot = new System.Object(); return _syncRoot; } }

    private void Awake()
    {
        lock (syncRoot)
        {
            if (instance == null)
            {
                //If I am the first instance, make me the Singleton
                instance = this as T;
                AwakeSingleton();
            }
            else
            {
                //If a Singleton already exists and you find
                //another reference in scene, destroy it!
                if (this != instance)
                {
                    DestroyImmediate(this.gameObject);
                }
            }
        }
    }

    public static T Instance
    {
        get
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                }
                return instance;
            }
        }
    }

    public void OnApplicationQuit()
    {
        OnApplicationQuitSingleton();
        instance = null;
    }

    /// <summary>
    /// Called when application quits. Don't use OnApplicationQuit(), use this instead.
    /// </summary>
    public virtual void OnApplicationQuitSingleton() { }

    /// <summary>
    /// Called when the behaviour is instantiated. Don't use Awake(), use this instead.
    /// </summary>
    public virtual void AwakeSingleton() { }
}

