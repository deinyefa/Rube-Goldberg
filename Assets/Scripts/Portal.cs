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
		newPosition = transform.forward * 7;
	}

	void Update () 
	{
		ShowNewPosition ();
	}

	void ShowNewPosition ()
	{
		path.SetPosition (0, gameObject.transform.position);
		path.SetPosition (1, newPosition);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Throwable"))
		{
			Destroy(this.gameObject);
			other.gameObject.transform.position = newPosition;
		}
	}
}
