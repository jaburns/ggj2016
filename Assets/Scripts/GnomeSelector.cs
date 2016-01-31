using UnityEngine;

public class GnomeSelector : MonoBehaviour
{
    static public bool gnomesEnabled = true;

    void Update()
    {
        if (!gnomesEnabled) return;

        var mouseDown = Input.GetMouseButtonDown(0);
        if (!mouseDown) return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GnomeController gnome = null;
        foreach (var item in Physics2D.OverlapPointAll(mousePosition)) {
            gnome = item.GetComponent<GnomeController>();
            if (gnome != null) break;
        }
        if (gnome == null) return;

        if (Input.GetKey(KeyCode.LeftShift)) {
            gnome.SetSelected(true);
        } else {
            foreach (var g in FindObjectsOfType<GnomeController>()) {
                g.SetSelected(g == gnome);
            }
        }
        SoundPlayer.Instance.Play("CreatureNoises");
    }
}
