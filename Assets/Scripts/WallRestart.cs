using UnityEngine;
using System.Collections;

public class WallRestart : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        MusicPlayer.Instance.Stop();
        SceneLoader.ReloadLevel();
    }
}
