using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Rigidbody2D _rb;
    bool _startedWriting;

    void Start ()
    {
        _rb = GetComponent<Rigidbody2D>();
        SkyBoxController.Instance.OnDusk.Sub(gameObject, OnDusk);
    }

    void OnDusk()
    {
        _rb.isKinematic = false;
    }

    IEnumerator writeRoutine()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale *= 0.9f;
        SkyBoxController.Instance.StartNight();
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
