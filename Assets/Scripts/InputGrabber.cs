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

    public const int SELECTION_KEYS_START_INDEX = 6;
    public const int SELECTION_KEYS_COUNT = 10;

    readonly KeyCode[] WATCH_KEYS = {
        KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.A, KeyCode.D, KeyCode.W,
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
    };

    GnomeInputState _curRedInputs;
    GnomeInputState _curYellowInputs;

    Button[] _selectionButtons;

    List<KeyCode> _keysDown;
    bool _inputWasProcessed;

    public Button[] SelectionButtons {
        get { return _selectionButtons; }
    }

    void Awake()
    {
        _keysDown = new List<KeyCode>();
        _selectionButtons = new Button[SELECTION_KEYS_COUNT];
    }

    public GnomeInputState GetInputsForColor(GnomeController.GnomeColor color)
    {
        switch (color) {
            case GnomeController.GnomeColor.Red: return _curRedInputs;
            case GnomeController.GnomeColor.Yellow: return _curYellowInputs;
        }
        return new GnomeInputState();
    }

    public GnomeInputState GetEmptyInputs()
    {
        return new GnomeInputState();
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

        for (int i = 0; i < SELECTION_KEYS_COUNT; ++i) {
            _selectionButtons[i] = _selectionButtons[i].Step(_keysDown.Contains(WATCH_KEYS[i + SELECTION_KEYS_START_INDEX]));
        }

        _inputWasProcessed = true;
    }
}
