/*
 * This script handles the laser button press for the start and win scenes.
 * A laser points from the right controller and with the trigger press, when it's pointed 
 * at the button, the onClick event is called.
 * The reference for this script can be found at https://gitlab.com/alkamegames/laser-pointer-button-click-for-htcvive
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserButtonClicker : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private SteamVR_LaserPointer laserPointer;
    private Button button;
    private GameObject myEventSystem;
    private bool pointerOnButton = false;

	void Awake ()
    {
        trackedObj = GetComponentInParent<SteamVR_TrackedObject>();

        myEventSystem = GameObject.Find("EventSystem");
        laserPointer = GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += LaserPointer_PointerIn;
        laserPointer.PointerOut += LaserPointer_PointerOut;
    }
	
	void Update ()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (pointerOnButton)
        {
            if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                button.onClick.Invoke();
        }
    }

    void LaserPointer_PointerOut (object sender, PointerEventArgs e)
    {
        if (button != null)
        {
            pointerOnButton = false;
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            button = null;
        }
    }

    void LaserPointer_PointerIn (object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null && button == null)
        {
            button = e.target.gameObject.GetComponent<Button>();
            button.Select();
            pointerOnButton = true;
        }
    }
}
