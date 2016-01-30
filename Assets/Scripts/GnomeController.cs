using UnityEngine;

public class GnomeController : MonoBehaviour
{
    public enum GnomeColor {
        Red,
        Yellow
    }

    public GnomeColor MyColor;

    Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var inputs = InputGrabber.Instance.GetInputsForColor(MyColor);
        _rb.AddForce(10* Vector2.right * inputs.WalkAxis);
    }
}
