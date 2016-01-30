using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public float MaxRunSpeed;
    public float JumpImpulse;
    public float RunForce;
    public float TurnAroundMultiplier;
    public float DecelMultiplier;
    public GameObject SelectionObject;

    public enum GnomeColor {
        Red,
        Yellow
    }

    public GnomeColor MyColor;

    Rigidbody2D _rb;
    GnomeController _onHead;
    bool _selected;
    bool _grounded;

    void Awake()
    {
        SelectionObject.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _selected = false;
    }

    public void SetSelected(bool selected)
    {
        _selected = selected;
        SelectionObject.SetActive(_selected);
    }

    void FixedUpdate()
    {
        var inputs = _selected
            ? InputGrabber.Instance.CurrentGnomeInputs
            : InputGrabber.EmptyInputs;

        var runForce = RunForce * Vector2.right * inputs.WalkAxis;
        if (runForce.x * _rb.velocity.x < 0) runForce *= TurnAroundMultiplier;
        _rb.AddForce(runForce);

        if (_onHead) {
            var hb = _onHead.GetComponent<Rigidbody2D>();
            hb.AddForce(runForce / _rb.mass * hb.mass);
        }

        if (inputs.JumpButton.JustPressed && _grounded) {
            _rb.AddForce(JumpImpulse * Vector2.up, ForceMode2D.Impulse);
            SoundPlayer.Instance.Play("Jump");
            _grounded = false;
        }

        if (Mathf.Abs(_rb.velocity.x) > MaxRunSpeed) {
            _rb.velocity = _rb.velocity.WithX(MaxRunSpeed * Mathf.Sign(_rb.velocity.x));
        }

        if (Mathf.Abs(inputs.WalkAxis) < .1f) {
            _rb.velocity = _rb.velocity.WithX(_rb.velocity.x * DecelMultiplier);
        }

        _onHead = null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (var contact in col.contacts) {
            if (contact.normal.y > 0.8f) _grounded = true;
        }

        OnCollisionStay2D(col);
    }

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
