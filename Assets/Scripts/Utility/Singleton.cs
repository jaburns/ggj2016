using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T s_instance;
    static bool s_quitting;

    static public T Instance
    {
        get {
            if (s_quitting) return null;
            if (s_instance != null) return s_instance;

            s_instance = FindObjectOfType<T>();

            if (s_instance == null) {
                var go = new GameObject();
                s_instance = go.AddComponent<T>();
                go.name = "[Singleton] " + typeof(T);
            }

            return s_instance;
        }
    }

    protected bool KeepAcrossScenes()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<SceneLoader>().Length > 1) {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void OnApplicationQuit()
    {
        s_quitting = true;
    }
}
