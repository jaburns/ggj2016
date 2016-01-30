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
            var newState = new GnomeInputState {
                LeftButton = LeftButton.Step(left),
                RightButton = RightButton.Step(right),
                JumpButton = JumpButton.Step(up),
            };

            newState.WalkAxis = newState.RightButton.Pressing ? 1f : newState.LeftButton.Pressing ? -1f : 0;

            return newState;
        }
    }

    readonly KeyCode[] WATCH_KEYS = {
        KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.Tab
    };

    GnomeInputState _curRedInputs;
    GnomeInputState _curYellowInputs;

    public Button SelectionButton { get; private set; }

    List<KeyCode> _keysDown;
    bool _inputWasProcessed;

    void Awake()
    {
        _keysDown = new List<KeyCode>();
    }

    public GnomeInputState GetInputsForColor(GnomeController.GnomeColor color)
    {
        switch (color) {
            case GnomeController.GnomeColor.Red: return _curRedInputs;
            case GnomeController.GnomeColor.Yellow: return _curYellowInputs;
        }
        return new GnomeInputState();
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
        _curRedInputs = _curRedInputs.Step(
            _keysDown.Contains(KeyCode.LeftArrow),
            _keysDown.Contains(KeyCode.RightArrow),
            _keysDown.Contains(KeyCode.UpArrow));

        _curYellowInputs = _curYellowInputs.Step(
            _keysDown.Contains(KeyCode.A),
            _keysDown.Contains(KeyCode.D),
            _keysDown.Contains(KeyCode.W));

        SelectionButton = SelectionButton.Step(_keysDown.Contains(KeyCode.Tab));

        _inputWasProcessed = true;
    }
}
