using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    Vector3 initialPosition;
    Rigidbody rigidBody;
    public Material outsidePlayspaceMaterial;
    Material insidePlayspaceMaterial;
    int layerMask;
    Goal goal;

    // Use this for initialization
    void Start () {
        initialPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        insidePlayspaceMaterial = GetComponent<Renderer>().material;
		goal = GameObject.FindObjectOfType<Goal> ();
	}

    private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Ground")) {
			transform.position = initialPosition;
			rigidBody.velocity = Vector3.zero;
			GetComponent<Renderer> ().material = insidePlayspaceMaterial;
			foreach (GameObject collectable in goal.collectables) {
				collectable.GetComponent<MeshCollider> ().enabled = true;
			}

			for (int i = 0; i < goal.collectables.Length; i++) {
				goal.collectables [i].SetActive (true);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
        foreach (GameObject collectable in goal.collectables)
        {
            if (other.gameObject.CompareTag("Collectable"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
    

    /* ----------------------------------------------------------------------------------- 
                                          Anti-cheat Code
       ----------------------------------------------------------------------------------- */

        public void AntiCheat()
        {
            layerMask = 1 << 8;
            // ToDo: change layer whenever ball leaves the platform
            layerMask = ~layerMask;
        
        RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
            //- outside playspace
            foreach (GameObject collectable in goal.collectables)
            {
                collectable.GetComponent<MeshCollider>().enabled = false;
            }
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                GetComponent<Renderer>().material = outsidePlayspaceMaterial;
            }
            else
            {
            //- inside playspace
            foreach (GameObject collectable in goal.collectables)
            {
                collectable.GetComponent<MeshCollider>().enabled = true;
            }
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                Debug.Log("Did not Hit");
                GetComponent<Renderer>().material = insidePlayspaceMaterial;
            }
        }
}
