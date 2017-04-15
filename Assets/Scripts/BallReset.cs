using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    Vector3 initialPosition;
    Rigidbody rigidBody;
    public GameObject[] collectables;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        LoadNewLevel();
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            transform.position = initialPosition;
            rigidBody.velocity = Vector3.zero;

            for (int i = 0; i < collectables.Length; i++)
            {
                collectables[i].SetActive(true);
            }
        }

        foreach (GameObject collectable in collectables)
        {
            if (other.gameObject.CompareTag("Collectable"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    void LoadNewLevel ()
    {
        if (AreStarsInActive())
        {
            Debug.Log("loading Scene 1");
            SteamVR_LoadLevel.Begin("Scene1");
        }
    }

    bool AreStarsInActive ()
    {
        foreach (GameObject collectable in collectables)
        {
            if (collectable.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
