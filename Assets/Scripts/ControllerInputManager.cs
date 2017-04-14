using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    //- to teleport
    private LineRenderer laser;
    private float yNudgeAmount = 0.1f;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask layerMask;

    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        Teleport();
	}

    void Teleport ()
    {
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);

            laser.SetPosition(0, gameObject.transform.position);
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerMask))
            {
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
            } else
            {
                teleportLocation = new Vector3(transform.forward.x * 5 + transform.position.x, transform.forward.y * 5 + transform.position.y, transform.forward.z * 5 + transform.position.z);
                RaycastHit groundRay;

                if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 7, layerMask))
                {
                    teleportLocation = new Vector3(transform.forward.x * 5 + transform.position.x, groundRay.point.y, transform.forward.z * 5 + transform.position.z);
                }
                laser.SetPosition(1, transform.forward * 5 + transform.position);
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);
            }
        }

/*        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
            player.transform.position = teleportLocation;
        }
*/
    }
}
