using UnityEngine;

public class GnomeSelector : MonoBehaviour
{
    GnomeController[] _allGnomes;
    int _maxIndex;
    int _curIndex;
    int _curIndex2;

    void Start()
    {
        _allGnomes = FindObjectsOfType<GnomeController>();

        _curIndex = 0;
        _maxIndex = -1;
        foreach (var gnome in _allGnomes) {
            if (gnome.SelectionIndex == 0) {
                gnome.SetSelected(true);
            }
            if (gnome.SelectionIndex > _maxIndex) {
                _maxIndex = gnome.SelectionIndex;
            }
        }
    }

    void FixedUpdate ()
    {
        if (InputGrabber.Instance.SelectionButton.JustPressed) {
            _curIndex = (_curIndex + 1) % (_maxIndex + 1);
        }
        if (InputGrabber.Instance.SelectionButton2.JustPressed) {
            _curIndex2 = (_curIndex2 + 1) % (_maxIndex + 1);
        }

        foreach (var gnome in _allGnomes) {
            if (gnome.MyColor == GnomeController.GnomeColor.Red) {
                gnome.SetSelected(gnome.SelectionIndex == _curIndex);
            } else {
                gnome.SetSelected(gnome.SelectionIndex == _curIndex2);
            }
        }
    }
}
