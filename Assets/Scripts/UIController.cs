using UnityEngine;

public class UIController : MonoBehaviour
{
    public void RestartClick()
    {
        MusicPlayer.Instance.Stop();
        SceneLoader.ReloadLevel();
    }
}
