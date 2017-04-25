using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRContollerInputManager : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    [Header("Teleporting")]
    //- to teleport
    public bool isLeftController;
    private LineRenderer laser;
    private float yNudgeAmount = 0.1f;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask layerMask;

    [Header("Grabbing and Throwing")]
    //- grabbing and throwing
    private OVRInput.Controller thisController;
    private float throwForce = 1.5f;

    [Header("Object Menu")]
    //- object menu
    public bool isRightController;
    private float swipeSum;
    private float touchLast;
    private float touchCurrent;
    private float distance;
    private bool hasSwipedLeft;
    private bool hasSwipedRight;
    public ObjectMenuManager objectMenuManager;
    private bool menuIsSwipeable;
    private float menuStickX;

	private ObjectController bridge;
	private ObjectController trampoline;
	private ObjectController wood_plank;
	private ObjectController portal;
	private ObjectController metal_plank;

    //------------------------------------------------------------------------------------------------------

    void Start()
    {
        laser = GetComponentInChildren<LineRenderer>();

        if (isLeftController)
        {
            thisController = OVRInput.Controller.LTouch;
        } else
        {
            thisController = OVRInput.Controller.RTouch;
        }

		bridge = new ObjectController ();
		trampoline = new ObjectController ();
		wood_plank = new ObjectController ();
		portal = new ObjectController ();
		metal_plank = new ObjectController ();

		bridge.InitializeMaxCount (-1, 3, 3, 1);
		trampoline.InitializeMaxCount (-1, 3, 1, 1);
		wood_plank.InitializeMaxCount (-1, 3, 1, 1);
		portal.InitializeMaxCount (-1, 3, 1, 1);
		metal_plank.InitializeMaxCount (-1, 3, 1, 1);
    }

    void Update()
    {
        Teleport();
        ObjectMenu();
    }

    /* -------------------------------------------------------------------------------------------------------- //

                                                    Teleporting
    // -------------------------------------------------------------------------------------------------------- */

    void Teleport()
    {
        if (isLeftController)
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
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
                }
                else
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

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
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
    

    void GrabObject(Collider col)
    {
        col.transform.SetParent(gameObject.transform);
        col.GetComponent<Rigidbody>().isKinematic = true;
    }

    void ThrowObject(Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidbody = col.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.velocity = OVRInput.GetLocalControllerVelocity(thisController) * throwForce;
        rigidbody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(thisController);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) < 0.1f)
            {
                ThrowObject(other);
            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) > 0.1f)
            {
                GrabObject(other);
            }
        }

        if (other.gameObject.CompareTag("Structure"))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) < 0.1f)
            {
                other.transform.SetParent(null);
                Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) > 0.1f)
            {
                GrabObject(other);
            }
        }
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------- //

                                                                                   Object Menu
   // -------------------------------------------------------------------------------------------------------------------------------------------------------- */

    void ObjectMenu()
    {
        if (isLeftController)
        {
            menuStickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, thisController).x;
            if (menuStickX < 0.45f && menuStickX > -0.45f)
            {
                menuIsSwipeable = true;
            }
            if (menuIsSwipeable)
            {
                if (menuStickX >= 0.45f)
                {
                    objectMenuManager.MenuRight();
                    menuIsSwipeable = false;
                }
                else if (menuStickX <= -0.45f)
                {
                    objectMenuManager.MenuLeft();
                    menuIsSwipeable = false;
                }
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
        {
			SpawnObject ();
        }
    }

    void SwipeRight()
    {
        objectMenuManager.MenuRight();
    }

    void SwipeLeft()
    {
        objectMenuManager.MenuLeft();
    }

    void SpawnObject()
    {
		switch (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject].name) {
		case "Bridge":
			bridge.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "Trampoline":
			trampoline.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "Wood_Plank":
			wood_plank.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "Portal":
			portal.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		case "Metal_Plank":
			metal_plank.SpawnObject (objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
				objectMenuManager.objectList[objectMenuManager.currentObject].transform);
			break;
		}
    }
}
