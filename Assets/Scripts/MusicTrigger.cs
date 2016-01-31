using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
	void OnTriggerEnter2D()
	{
	    SoundPlayer.Instance.Play("MusicTriggers");
    }
}
