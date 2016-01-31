using UnityEngine;

public class LevelTime : MonoBehaviour
{
    public float LevelLength = 10f;

    void Awake()
    {
        var fader = GameObject.Find("Faderz");
        fader.transform.position = fader.transform.position.WithXY(0, 0);
    }
}
