using UnityEngine;

public class GnomeSelector : MonoBehaviour
{
    void Update()
    {
        var mouseDown = Input.GetMouseButtonDown(0);
        if (!mouseDown) return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var overlap = Physics2D.OverlapPoint(mousePosition);
        if (overlap == null) return;

        var gnome = overlap.GetComponent<GnomeController>();
        if (gnome == null) return;

        if (Input.GetKey(KeyCode.LeftShift)) {
            gnome.SetSelected(true);
        } else {
            foreach (var g in FindObjectsOfType<GnomeController>()) {
                g.SetSelected(g == gnome);
            }
        }
    }
}
