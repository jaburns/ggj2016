using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Rigidbody2D _rb;
    bool _startedWriting;
    bool _done;

    void Start ()
    {
        GnomeSelector.gnomesEnabled = false;
        _rb = GetComponent<Rigidbody2D>();

        SkyBoxController.Instance.OnDusk.Sub(gameObject, OnDusk);
        SkyBoxController.Instance.OnNightEnd.Sub(gameObject, OnNightEnd);
    }

    void OnDusk()
    {
        _rb.isKinematic = false;
    }

    void OnNightEnd()
    {
        if (_done) return;
        _done = true;
        StartCoroutine(angryRoutine());
    }

    public void GnomesWin()
    {
        if (_done) return;
        _done = true;
        StartCoroutine(happyRoutine());
    }

    IEnumerator happyRoutine()
    {
        transform.localScale *= 0.5f;
        GnomeSelector.gnomesEnabled = false;
        yield return new WaitForSeconds(1f);
        SceneLoader.NextLevel();
    }

    IEnumerator writeRoutine()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale *= 0.9f;
        SkyBoxController.Instance.StartNight(FindObjectOfType<LevelTime>().LevelLength);
        GnomeSelector.gnomesEnabled = true;
    }

    IEnumerator angryRoutine()
    {
        transform.localScale *= 1.5f;
        GnomeSelector.gnomesEnabled = false;
        yield return new WaitForSeconds(1f);
        SceneLoader.ReloadLevel();
    }

    void OnCollisionEnter2D()
    {
        if (!_startedWriting) {
            _startedWriting = true;
            SkyBoxController.Instance.ShowCracks();
            StartCoroutine(writeRoutine());
        }
    }
}
