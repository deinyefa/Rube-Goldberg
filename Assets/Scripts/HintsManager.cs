using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class HintsManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public HintVariables hintVariable;
    public Text tutorialText;
    public GameObject controllerTextHint;
    public bool isLeftController;
    public bool isRightController;

    void Awake ()
    {
        trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
	}
	
	void Update ()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        DisplayHints();
    }

    void DisplayHints ()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (isLeftController)
            {
                //- loads the teleportation hint when user touches the touchpad
                if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    tutorialText.text = "To teleport, hold and\nrelease the touchpad";
                }
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    tutorialText.text = "Touch the ball with either\ncontroller and hold the trigger\nto lift it";
                    hintVariable.HasTeleported = true;
                }
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    hintVariable.BallHasBeenLifted = true;
                }
                if (hintVariable.BallHasBeenLifted == true && hintVariable.HasTeleported == true)
                {
                    tutorialText.text = "Press and hold\nthe trigger on the right\ncontroller to continue";
                    hintVariable.LeftControllerTutorialCompleted = true;
                }
            }
            if (isRightController)
            {
                //- can only complete right controller tutorial if player has completed left controller tutorial
                if (hintVariable.LeftControllerTutorialCompleted == true)
                {
                    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        tutorialText.text = "To choose a tool,\ntouch the touchpad while\npressing the trigger.\nMove the tool like\nyou would the ball";
                    }
                    if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        hintVariable.RightControllerTutorialConpleted = true;
                    }
                }
            }
            if (hintVariable.LeftControllerTutorialCompleted == true && hintVariable.RightControllerTutorialConpleted == true)
            {
                if (isLeftController)
                    controllerTextHint.SetActive(false);
                if (isRightController)
                    controllerTextHint.SetActive(false);
            }
        }
    }
}
