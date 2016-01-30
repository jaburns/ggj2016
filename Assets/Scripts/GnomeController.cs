using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public float MaxRunSpeed;
    public float JumpImpulse;
    public float RunForce;
    public float TurnAroundMultiplier;
    public float DecelMultiplier;

    public enum GnomeColor {
        Red,
        Yellow
    }

    public GnomeColor MyColor;

    Rigidbody2D _rb;
    GnomeController _onHead;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var inputs = InputGrabber.Instance.GetInputsForColor(MyColor);

        var runForce = RunForce * Vector2.right * inputs.WalkAxis;
        if (runForce.x * _rb.velocity.x < 0) runForce *= TurnAroundMultiplier;
        _rb.AddForce(runForce);

        if (_onHead) {
            var hb = _onHead.GetComponent<Rigidbody2D>();
            hb.AddForce(runForce / _rb.mass * hb.mass);
        }

        if (inputs.JumpButton.JustPressed) {
            _rb.AddForce(JumpImpulse * Vector2.up, ForceMode2D.Impulse);
        }

        if (Mathf.Abs(_rb.velocity.x) > MaxRunSpeed) {
            _rb.velocity = _rb.velocity.WithX(MaxRunSpeed * Mathf.Sign(_rb.velocity.x));
        }

        if (Mathf.Abs(inputs.WalkAxis) < .1f) {
            _rb.velocity = _rb.velocity.WithX(_rb.velocity.x * DecelMultiplier);
        }

        _onHead = null;
    }

    void OnCollisionEnter2D(Collision2D col) { OnCollisionStay2D(col); }
    void OnCollisionStay2D(Collision2D col)
    {
        var colGnome = col.collider.gameObject.GetComponent<GnomeController>();
        if (colGnome == null) return;

        bool isTop = false;
        foreach (var contact in col.contacts) {
            if (contact.normal.y < -0.8f) isTop = true;
        }
        if (!isTop) return;

        _onHead = colGnome;
    }
}
