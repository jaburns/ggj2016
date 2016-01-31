using System.Collections;
using UnityEngine;

public class StartBlock : MonoBehaviour
{
    public bool JustSound;

    Vector3 _originalPos;
    float _bounceT;
    bool _started;

    void Awake()
    {
        GnomeSelector.gnomesEnabled = true;
        _bounceT = 1f;
        _originalPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        bool fromBottom = false;
        foreach (var contact in col.contacts) {
            if (contact.normal.y > 0.8f) {
                fromBottom = true;
                break;
            }
        }
        if (!fromBottom) return;

        SoundPlayer.Instance.Play("BlockHit");

        if (JustSound) {
        } else if (!_started) {
            _started = true;
            StartCoroutine(waitAndLoad());
        }
        _bounceT = 0;
    }

    IEnumerator waitAndLoad()
    {
        yield return new WaitForSeconds(.3f);
        SceneLoader.StartGame();
    }

    void Update()
    {
        if (_bounceT < 1) {
            _bounceT += 5*Time.deltaTime;
        } else {
            _bounceT = 1;
        }
        transform.position = _originalPos + 0.2f*Vector3.up*Mathf.Sin(10*_bounceT/Mathf.PI);
    }
}
