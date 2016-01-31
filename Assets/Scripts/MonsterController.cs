using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Rigidbody2D _rb;
    bool _startedWriting;

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
        StartCoroutine(angryRoutine());
    }

    IEnumerator writeRoutine()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale *= 0.9f;
        SkyBoxController.Instance.StartNight();
        GnomeSelector.gnomesEnabled = false;
    }

    IEnumerator angryRoutine()
    {
        GnomeSelector.gnomesEnabled = false;
        transform.localScale *= 1.5f;
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(Application.loadedLevelName);
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
