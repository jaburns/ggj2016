using UnityEngine;

public class InputGrabber : Singleton<InputGrabber>
{
    public struct Button
    {
        public bool JustPressed;
        public bool Pressing;
        public bool JustReleased;

        public Button Step(bool pressedNow)
        {
            return new Button {
                JustPressed = !Pressing && pressedNow,
                Pressing = pressedNow,
                JustReleased = Pressing && !pressedNow
            };
        }
    }

    public struct GnomeInputState
    {
        public bool Walking;
        public float WalkAxis;
        public Button JumpButton;

        public GnomeInputState UpdateWithKeys(KeyStates keys)
        {
            var walkAxis = keys.LeftKey ? -1f : keys.RightKey ? 1f : 0f; // : Input.GetAxis("Horizontal");

            return new GnomeInputState {
                Walking = Mathf.Abs(walkAxis) > 0.1f,
                WalkAxis = walkAxis,
                JumpButton = JumpButton.Step(keys.JumpKey)
            };
        }
    }

    public struct KeyStates
    {
        public bool LeftKey;
        public bool RightKey;
        public bool JumpKey;

        public KeyStates UpdateWithInputs(bool left, bool right, bool up)
        {
            return new KeyStates {
                LeftKey = LeftKey | left,
                RightKey = RightKey | right,
                JumpKey = JumpKey | up
            };
        }
    }

    GnomeInputState _curRedInputs;
    KeyStates _curRedKeys;
    GnomeInputState _curYellowInputs;
    KeyStates _curYellowKeys;

    bool _inputWasProcessed;

    public GnomeInputState GetInputsForColor(GnomeController.GnomeColor color)
    {
        switch (color) {
            case GnomeController.GnomeColor.Red: return _curRedInputs;
            case GnomeController.GnomeColor.Yellow: return _curYellowInputs;
        }
        return new GnomeInputState();
    }

    void Update()
    {
        if (_inputWasProcessed) {
            _inputWasProcessed = false;
            _curRedKeys = new KeyStates();
            _curYellowKeys = new KeyStates();
        }

        _curRedKeys.UpdateWithInputs(
            Input.GetKey(KeyCode.LeftArrow),
            Input.GetKey(KeyCode.RightArrow),
            Input.GetKey(KeyCode.UpArrow));

        _curYellowKeys.UpdateWithInputs(
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.W));
    }

    void FixedUpdate()
    {
        _curRedInputs = _curRedInputs.UpdateWithKeys(_curRedKeys);
        _curYellowInputs = _curYellowInputs.UpdateWithKeys(_curYellowKeys);
        _inputWasProcessed = true;
    }
}
