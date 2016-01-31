using System.Collections.Generic;
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
        public Button LeftButton;
        public Button RightButton;
        public Button JumpButton;
        public float WalkAxis;

        public GnomeInputState Step(bool left, bool right, bool up)
        {
            up = up? up : MobileController.jumpButtonPressed;
            MobileController.jumpButtonPressed = false;
            var newState = new GnomeInputState {
                LeftButton = LeftButton.Step(left),
                RightButton = RightButton.Step(right),
                JumpButton = JumpButton.Step(up),
            };

            newState.WalkAxis = (newState.RightButton.Pressing || MobileController.rightButtonPressed) ? 1f : 
                (newState.LeftButton.Pressing || MobileController.leftButtonPressed) ? -1f : 0;

            return newState;
        }
    }

    readonly KeyCode[] WATCH_KEYS = {
        KeyCode.A, KeyCode.D, KeyCode.W
    };

    GnomeInputState _curInputs;
    List<KeyCode> _keysDown;
    bool _inputWasProcessed;

    void Awake()
    {
        _keysDown = new List<KeyCode>();
    }

    public GnomeInputState CurrentGnomeInputs
    {
        get {
            return _curInputs;
        }
    }

    static public GnomeInputState EmptyInputs
    {
        get {
            return new GnomeInputState();
        }
    }

    void Update()
    {
        if (_inputWasProcessed) {
            _inputWasProcessed = false;
            _keysDown.Clear();
        }

        foreach (var key in WATCH_KEYS) {
            if (!_keysDown.Contains(key) && Input.GetKey(key)) {
                _keysDown.Add(key);
            }
        }
    }

    void FixedUpdate()
    {
        _curInputs = _curInputs.Step(
            _keysDown.Contains(KeyCode.A),
            _keysDown.Contains(KeyCode.D),
            _keysDown.Contains(KeyCode.W));

        _inputWasProcessed = true;
    }
}
