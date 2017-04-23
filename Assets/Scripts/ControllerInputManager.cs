﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    public BallReset ballReset;
    public Material outsidePlayspaceMaterial;

    [Header( "Teleporting" )]
    //- to teleport
    public bool isLeftController;
    private LineRenderer laser;
    private float yNudgeAmount = 0.1f;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask layerMask;

    [Header( "Grabbing and Throwing" )]
    //- grabbing and throwing
    private float throwForce = 1.5f;

    [Header( "Object Menu" )]
    //- object menu
    public bool isRightController;
    private float swipeSum;
    private float touchLast;
    private float touchCurrent;
    private float distance;
    private bool hasSwipedLeft;
    private bool hasSwipedRight;
    public ObjectMenuManager objectMenuManager;

	private ObjectController bridge;
	private ObjectController cog;
	private ObjectController plank;
	private ObjectController portal;
	private ObjectController torus;


    //------------------------------------------------------------------------------------------------------

    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        
		laser = GetComponentInChildren<LineRenderer>();

		bridge = new ObjectController ();
		cog = new ObjectController ();
		plank = new ObjectController ();
		portal = new ObjectController ();
		torus = new ObjectController ();

		bridge.InitializeMaxCount (-1, 3, 3, 1);
		cog.InitializeMaxCount (-1, 3, 1, 1);
		plank.InitializeMaxCount (-1, 3, 1, 1);
		portal.InitializeMaxCount (-1, 3, 1, 1);
		torus.InitializeMaxCount (-1, 3, 1, 1);
	}
	
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        
		Teleport();
        ObjectMenu();
	}

    /* -------------------------------------------------------------------------------------------------------- //

                                                    Teleporting
    // -------------------------------------------------------------------------------------------------------- */

    void Teleport ()
    {
        if (isLeftController)
        {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                laser.gameObject.SetActive(true);
                teleportAimerObject.SetActive(true);

                laser.SetPosition(0, gameObject.transform.position);
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 15, layerMask))
                {
                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
                }
                else
                {
                    teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + 
                                                    transform.position.y, transform.forward.z * 15 + transform.position.z);
                    RaycastHit groundRay;

                    if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, layerMask))
                    {
                        teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, 
                                                        groundRay.point.y, transform.forward.z * 15 + transform.position.z);
                    }
                    laser.SetPosition(1, transform.forward * 15 + transform.position);
                    teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);
                }
            }

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);
                player.transform.position = teleportLocation;
            }
        }
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------- //

                                                                                    Grabbing and Throwing
    // -------------------------------------------------------------------------------------------------------------------------------------------------------- */

    void GrabObject (Collider col)
    {
        col.transform.SetParent(gameObject.transform);
        col.GetComponent<Rigidbody>().isKinematic = true;
        device.TriggerHapticPulse(1000);
    }

    void ThrowObject (Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidbody = col.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.velocity = device.velocity * throwForce;
        rigidbody.angularVelocity = device.angularVelocity;

        ballReset.AntiCheat();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                ThrowObject(other);
            }
            else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GrabObject(other);
            }
        }
        
            if (other.gameObject.CompareTag("Structure"))
            {
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    other.transform.SetParent(null);
                    Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                    rigidbody.isKinematic = false;
                }
                else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    GrabObject(other);
                }
            }
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------- //

                                                                                   Object Menu
   // -------------------------------------------------------------------------------------------------------------------------------------------------------- */

    void ObjectMenu ()
    {
        if (isRightController)
        {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                touchLast = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
            }
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
                touchCurrent = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
                distance = touchCurrent - touchLast;
                touchLast = touchCurrent;
                swipeSum += distance;

                if (!hasSwipedRight)
                {
                    if (swipeSum > 0.5f)
                    {
                        swipeSum = 0;
                        SwipeRight();
                        hasSwipedRight = true;
                        hasSwipedLeft = false;
                    }
                }

                if (!hasSwipedLeft)
                {
                    if (swipeSum < -0.5f)
                    {
                        swipeSum = 0;
                        SwipeLeft();
                        hasSwipedLeft = true;
                        hasSwipedRight = false;
                    }
                }
            }

            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                int currentObject = objectMenuManager.currentObject;
                objectMenuManager.objectList[currentObject].SetActive(false);

                swipeSum = 0;
                touchCurrent = 0;
                touchLast = 0;
                hasSwipedLeft = false;
                hasSwipedRight = false;
            }
            
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                SpawnObject();
            }
        }
    }

    void SwipeRight()
    {
        objectMenuManager.MenuRight();
    }

    void SwipeLeft ()
    {
        objectMenuManager.MenuLeft();
    }

    void SpawnObject ()
    {
		switch (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject].name) {
		case "bridge":
			bridge.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "cog":
			cog.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "plank":
			plank.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "portal":
			portal.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "torus":
			torus.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		}
    }
}
