using UnityEngine;
using System.Collections;

public class MobileController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
        if(leftButtonPressed)
        {
            LeftClick();
        }
        if(rightButtonPressed)
        {
            RightClick();
        }

        if(jumpButtonPressed)
        {
            JumpClick();
        }
	
    }

    public void LeftClick()
    {
        //Debug.Log("LeftClick");
    }

    public void RightClick()
    {
        //Debug.Log("RightClick");
    }

    public static bool jumpButtonPressed = false;  

    public void JumpClick()
    {
        //Debug.Log("JumpClick");
    }

    public void JumpClickDown()
    {
        jumpButtonPressed = true;
    }

    public void JumpClickUp()
    {
        jumpButtonPressed = false;
    }

    public static bool leftButtonPressed = false;

    public void LeftClickDown()
    {
        leftButtonPressed = true;
    }

    public void LeftClickUp()
    {
        leftButtonPressed = false;
    }

    public static bool rightButtonPressed = false;

    public void RightClickDown()
    {
        rightButtonPressed = true;
    }

    public void RightClickUp()
    {
        rightButtonPressed = false;
    }
}
