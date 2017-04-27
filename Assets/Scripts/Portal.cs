/*
 * Add line renderer to show the path the potral will take the ball
 * portal only takes the ball so many units forward... (7 units)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	private Vector3 newPosition;
	private LineRenderer path;

	void Awake () 
	{
		path = GetComponentInChildren<LineRenderer> ();
	}

	void Update () 
	{
		ShowNewPosition ();
	}

	void ShowNewPosition ()
	{
		path.SetPosition (0, gameObject.transform.position);
		path.SetPosition (1, new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1f));
		path.transform.Rotate (transform.rotation.x, transform.rotation.y, transform.rotation.z);
		newPosition = path.GetPosition (1);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Throwable"))
		{
			other.gameObject.transform.position = newPosition;
			other.transform.parent = null;
			other.GetComponent<Rigidbody> ().AddForce (transform.forward * 2);
		}
	}
}
