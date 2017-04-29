using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class HintsManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public bool isLeftController;
    public bool isRightController;

    private ControllerButtonHints contollerButtonHints;

    void Awake ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        contollerButtonHints = GetComponent<ControllerButtonHints>();
	}

    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void DisplayHints ()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (isLeftController)
            {
                //- loads the teleportation hint when user touches the touchpad
                if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    contollerButtonHints.textHintPrefab = Resources.Load("teleportation_hint") as GameObject;
                }
                //- removes the hint when finger leaves the touchpad
                else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    contollerButtonHints.textHintPrefab = null;
                }
                //- if user has teleported, show the hint that leades player to the objects menu
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    contollerButtonHints.textHintPrefab = Resources.Load("go_to_menu_hint") as GameObject;
                }
            }
            if (isRightController)
            {

            }
        }
    }
}
