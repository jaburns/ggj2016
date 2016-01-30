using UnityEngine;
using System.Collections;

public class FormChecker : MonoBehaviour
{
  public GameObject doneCube;
	
  public bool isChecked = false;

  void Update()
  {
    var formChecked = true;
    foreach (Transform sphereTransform in transform)
    {
      
      var sphere = sphereTransform.gameObject;

      var checker = sphere.GetComponent<SphereChecker>();
      if (checker != null)
      {
        
        if (!checker.isChecked)
        {
          formChecked = false;
          break;
        }
      }
    }
    if (formChecked)
    {
      doneCube.SetActive(true);
    } else
    {
      doneCube.SetActive(false);
    }
  }
}
