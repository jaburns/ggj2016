using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public void RestartClick()
    {
        SceneLoader.ReloadLevel();
    }
}
