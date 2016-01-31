using UnityEngine;
using System.Collections;

public class SphereChecker : MonoBehaviour
{
    const bool SHOW = false;

  public GameObject checkedObject;

  public GnomeColorRequirement requireColor;

  public enum GnomeColorRequirement
  {
    Any,
    Red,
    Yellow
  }

  int numberOfCollidingGnomes = 0;

  public bool isChecked
  {
    get
    {
      return numberOfCollidingGnomes > 0;
    }
  }

  void Awake()
  {
      if (!SHOW) {
          foreach (var obj in GetComponentsInChildren<MeshRenderer>()) {
              obj.enabled = false;
          }
      }
  }

  void Update()
  {
    if (isChecked)
    {
      checkedObject.SetActive(true);
    } else
    {
      checkedObject.SetActive(false);
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    var component = other.gameObject.GetComponent<GnomeController>();
    if (component != null)
    {
      if ((requireColor == GnomeColorRequirement.Red && component.MyColor == GnomeController.GnomeColor.Red) ||
         (requireColor == GnomeColorRequirement.Yellow && component.MyColor == GnomeController.GnomeColor.Yellow) ||
         requireColor == GnomeColorRequirement.Any)
      {
        numberOfCollidingGnomes++;
      }
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    var component = other.gameObject.GetComponent<GnomeController>();
    if (component != null)
    {
      if ((requireColor == GnomeColorRequirement.Red && component.MyColor == GnomeController.GnomeColor.Red) ||
         (requireColor == GnomeColorRequirement.Yellow && component.MyColor == GnomeController.GnomeColor.Yellow) ||
         requireColor == GnomeColorRequirement.Any)
      {
        numberOfCollidingGnomes--;
      }
    }
  }
}
