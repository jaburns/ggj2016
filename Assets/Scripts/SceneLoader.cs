using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    void Awake()
    {
        if (KeepAcrossScenes()) return;

        if (Application.loadedLevelName != "Loader") {
            Application.LoadLevelAdditive("Loader");
        } else {
            Application.LoadLevelAdditive("Main");
        }
    }
}
