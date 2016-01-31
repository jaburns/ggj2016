using UnityEngine;

public class GnomeController : MonoBehaviour
{
    const float JUMP_HEAD_BOOST = .9f;
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
    Animator _anim;
    bool _selected;
    bool _grounded;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _selected = false;
        _anim = GetComponentInChildren<Animator>();
    }

    public void SetSelected(bool selected)
    {
        if (_anim == null) {
        } else {
            _anim.SetBool("Selected", selected);
        }
        _selected = selected;
    }

    void FixedUpdate()
    {
        var inputs = _selected && GnomeSelector.gnomesEnabled
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
            if (_onHead) {
                var hb = _onHead.GetComponent<Rigidbody2D>();
                hb.AddForce(Vector2.up * JUMP_HEAD_BOOST, ForceMode2D.Impulse);
            }
            _rb.AddForce(JumpImpulse * Vector2.up, ForceMode2D.Impulse);
            //SoundPlayer.Instance.Play("Jump");
            _grounded = false;
        }

        if (Mathf.Abs(_rb.velocity.x) > MaxRunSpeed) {
            _rb.velocity = _rb.velocity.WithX(MaxRunSpeed * Mathf.Sign(_rb.velocity.x));
        }

        if (Mathf.Abs(inputs.WalkAxis) < .1f) {
            _rb.velocity = _rb.velocity.WithX(_rb.velocity.x * DecelMultiplier);
        }

        if(Mathf.Abs(_rb.velocity.x) > 1 && _grounded)
        {
            WalkSound();
        }

        _onHead = null;
        _grounded = false;
    }

    float redStepSoundInterval = .3f;
    float yellowStepSoundInterval = .2f;

    float lastStepTime = 0;

    void WalkSound()
    {
        float interval = MyColor==GnomeColor.Red? redStepSoundInterval: yellowStepSoundInterval;
        if(lastStepTime + interval < Time.time)
        {
            lastStepTime = Time.time;
            SoundPlayer.Instance.Play("Walking");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (var contact in col.contacts)
        {
            if (contact.normal.y > 0.8f)
            {
                _grounded = true;

                var gnome = col.gameObject.GetComponent<GnomeController>();
                if(gnome != null)
                {
                    if(gnome.MyColor == GnomeColor.Red)
                    {
                        SoundPlayer.Instance.Play("PlayerLandsOnPlayerBig");
                    }
                    else 
                    {
                        SoundPlayer.Instance.Play("PlayerLandsOnPlayer");
                    }

                } else {
                    SoundPlayer.Instance.Play("Landing");
                }
            }

        }
        OnCollisionStay2D(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        foreach (var contact in col.contacts) {
            if (contact.normal.y > 0.8f) _grounded = true;
        }

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
