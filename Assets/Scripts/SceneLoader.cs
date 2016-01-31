using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    void Awake()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1) {
            Destroy(gameObject);
            return;
        }

        if (Application.loadedLevelName != "Loader") {
            Application.LoadLevelAdditive("Loader");
        } else {
            Application.LoadLevelAdditive("Level_0");
        }
    }

    static public void NextLevel()
    {
        Application.LoadLevel("Level_1");
    }

    static public void StartGame()
    {
        Application.LoadLevel("Level_0");
    }

    static public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
}
