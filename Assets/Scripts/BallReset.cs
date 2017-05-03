﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    Vector3 initialPosition;
    Rigidbody rigidBody;
    public Material outsidePlayspaceMaterial;
    [HideInInspector]
    public Material insidePlayspaceMaterial;
    int layerMask;
    Goal goal;

	void Awake () 
	{
        rigidBody = GetComponent<Rigidbody>();
        insidePlayspaceMaterial = GetComponent<Renderer>().material;
		goal = GameObject.FindObjectOfType<Goal> ();
	}

	void Start () 
	{
		initialPosition = transform.position;
	}

    private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Ground"))
		{
			transform.position = initialPosition;
			rigidBody.velocity = Vector3.zero;

			GetComponent<Renderer> ().material = insidePlayspaceMaterial;

			foreach (GameObject collectable in goal.collectables) 
			{
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
}
