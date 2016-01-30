using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
  public GameObject MainMenuScreen;
  public GameObject OptionsMenuScreen;

  public void StartButtonClick()
  {
    SceneManager.LoadScene(1);
  }

  public void OptionsButtonClick()
  {
    MainMenuScreen.SetActive(false);
    OptionsMenuScreen.SetActive(true);
  }

  public void QuitButtonClick() 
  {
    Application.Quit();
  }

  public void OptionsBackButtonClick()
  {
    MainMenuScreen.SetActive(true);
    OptionsMenuScreen.SetActive(false);
  }
}
