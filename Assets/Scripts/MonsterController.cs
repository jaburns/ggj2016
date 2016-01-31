using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Rigidbody2D _rb;

    void Start ()
    {
        _rb = GetComponent<Rigidbody2D>();
        SkyBoxController.Instance.OnDusk.Sub(gameObject, OnDusk);
    }

    void OnDusk()
    {
        _rb.isKinematic = false;
    }
}
