using UnityEngine;

public class GnomeSelector : MonoBehaviour
{
    GnomeController[] _allGnomes;
    int _maxIndex;
    int _curIndex;

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

        foreach (var gnome in _allGnomes) {
            gnome.SetSelected(gnome.SelectionIndex == _curIndex);
        }
    }
}
