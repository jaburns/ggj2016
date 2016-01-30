using UnityEngine;
using System.Collections;

public class PointChecker : MonoBehaviour
{
  public GameObject checkedObject;

  public GnomeColorRequirement requireColor;
  public enum GnomeColorRequirement {
    Any,
    Red,
    Yellow
  }
   
  int numberOfCollidingGnomes = 0;

  void Update()
  {
    if(numberOfCollidingGnomes > 0)
    {
      checkedObject.SetActive(true);
    } else {
      checkedObject.SetActive(false);
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    var compoment = other.gameObject.GetComponent<GnomeController>();
    if (compoment != null)
    {
      if(requireColor == GnomeColorRequirement.Red){
        if(compoment.MyColor == GnomeController.GnomeColor.Red)
        {
          numberOfCollidingGnomes++;
        }
      }
      else if(requireColor == GnomeColorRequirement.Yellow)
      {
        if(compoment.MyColor == GnomeController.GnomeColor.Yellow)
        {
          numberOfCollidingGnomes++;
        }
      } else {
        numberOfCollidingGnomes++;
      }
      Debug.Log("Colliding2");
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    var compoment = other.gameObject.GetComponent<GnomeController>();
    if (compoment != null)
    {
      if(requireColor == GnomeColorRequirement.Red){
        if(compoment.MyColor == GnomeController.GnomeColor.Red)
        {
          numberOfCollidingGnomes--;
        }
      }
      else if(requireColor == GnomeColorRequirement.Yellow)
      {
        if(compoment.MyColor == GnomeController.GnomeColor.Yellow)
        {
          numberOfCollidingGnomes--;
        }
      } else {
        numberOfCollidingGnomes--;
      }
      Debug.Log("Colliding2");
    }
  }
}
