using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Vector3 newPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Throwable"))
        {
            Destroy(this.gameObject);
            other.gameObject.transform.position = newPosition;
        }
    }
}
