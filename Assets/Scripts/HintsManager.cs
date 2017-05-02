using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class HintsManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private bool hasTeleported = false;
    private bool ballHasBeenLifted = false;
    private bool hasCompletedLeftControllerTutorial = false;
    private bool hasCompletedRightControllerTutorial = false;

    public bool isLeftController;
    public bool isRightController;
    public Text tutorialText;

    void Awake ()
    {
        trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
        tutorialText = GetComponentInChildren<Text>();
	}

    void Start ()
    {

    }
	
	// Update is called once per frame
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
                    hasTeleported = true;
                }
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    ballHasBeenLifted = true;
                }
                if (ballHasBeenLifted && hasTeleported)
                {
                    tutorialText.text = "Press the trigger on the right\ncontroller to continue";
                    hasCompletedLeftControllerTutorial = true;
                    Debug.Log(hasCompletedLeftControllerTutorial);
                }
            }
            if (isRightController)
            {
                
                if (ballHasBeenLifted && hasTeleported)
                {
                    tutorialText.text = "With the right controller,\nswipe left or right\nto choose a tool";
                }
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    tutorialText.text = "To choose a tool,\ntouch the touchpad and\npress the trigger.\nMove the tool like\nyou would the ball";

                }
                //- can only complete right controller tutorial if player has completed left controller tutorial
                if (hasCompletedLeftControllerTutorial)
                {
                    hasCompletedRightControllerTutorial = true;
                    if (hasCompletedLeftControllerTutorial && hasCompletedRightControllerTutorial)
                    {
                        Debug.Log(hasCompletedLeftControllerTutorial);

                        if (isLeftController)
                            tutorialText.gameObject.SetActive(false);
                        if (isRightController)
                            tutorialText.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
