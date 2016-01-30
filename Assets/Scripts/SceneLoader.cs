using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    void Awake()
    {
        Application.LoadLevelAdditive("Main");
    }
}
