using UnityEngine;


//Generic singleton construct - might be overcomplicated?
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    private static bool appQuitting = false;

    private static object instanceLock = new object();

    public static T Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (appQuitting)
                {
                    Debug.LogWarning("Application is quitting, will not create or retrieve instance of singleton: " + typeof(T) + ", returning null");
                    return null;
                }

                if (instance == null)
                {
                    var potentialObjects = FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None);
                    if (potentialObjects.Length > 1)
                    {
                        Debug.LogError("Singleton: THERE IS MORE THAN 1 INSTANCE OF SINGLETON TYPE (" + typeof(T) + ") IN SCENE!!");
                    }
                    else if (potentialObjects.Length > 0)
                    {
                        instance = (T)potentialObjects[0];
                    }
                }

                if (instance == null)
                {
                    Debug.Log("Singleton: No instance of type - " + typeof(T) + ", found in scene, creating default Singleton");
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<T>();
                    singleton.name = "Singleton - " + typeof(T).ToString();
                }

                return instance;
            }
        }
    }

    protected void Awake()
    {
        appQuitting = false;
        if (instance != null && instance != this)
        {
            Debug.LogError($"Error: Duplicate singleton {this.GetType()} detected, destroying duplicate. Ensure only 1 of each singleton is in the scene");
            Destroy(gameObject);
            return;
        }
        instance = this as T;

        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void OnApplicationQuit()
    {
        instance = null;
        appQuitting = true;
    }
}
