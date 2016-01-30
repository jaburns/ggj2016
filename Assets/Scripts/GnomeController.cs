using UnityEngine;
using System.Collections;

public class GnomeController : MonoBehaviour
{
    public enum GnomeColor {
        Red,
        Yellow
    }

    public GnomeColor MyColor;

    void FixedUpdate()
    {
        var inputs = InputGrabber.Instance.GetInputsForColor(MyColor);
    }
}
