using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    Vector3 initialPosition;
    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            transform.position = initialPosition;
            rigidBody.velocity = new Vector3(0, 0, 0);
        }
    }
}
