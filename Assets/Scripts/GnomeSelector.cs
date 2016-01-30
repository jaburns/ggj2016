using UnityEngine;

public class GnomeSelector : MonoBehaviour
{
    GnomeController[] _allGnomes;

    void Start()
    {
        _allGnomes = FindObjectsOfType<GnomeController>();
    }

    void Update()
    {
        var mouseDown = Input.GetMouseButtonDown(0);
        if (!mouseDown) return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var gnome = Physics2D.OverlapPoint(mousePosition).GetComponent<GnomeController>();
        if (gnome == null) return;

        foreach (var g in _allGnomes) {
            if (g.MyColor == gnome.MyColor) {
                g.SetSelected(g == gnome);
            }
        }
    }
}
