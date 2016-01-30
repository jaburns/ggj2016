using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Application.loadedLevelName != "Loader") {
            Application.LoadLevelAdditive("Loader");
        } else {
            Application.LoadLevelAdditive("Main");
        }
    }
}
