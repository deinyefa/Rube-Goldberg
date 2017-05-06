using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectMenuManager : MonoBehaviour {

    public List<GameObject> objectList;
    public List<GameObject> objectListPrefabs;

    [HideInInspector]
    public int currentObject = 0;

	void Start () {
		foreach (Transform child in transform)
        {
            objectList.Add(child.gameObject);
        }
	}
	
	public void MenuLeft ()
    {
        objectList[currentObject].SetActive(false);
        currentObject--;

        if(currentObject < 0)
        {
            currentObject = objectList.Count - 1;
        }
        objectList[currentObject].SetActive(true);
    }

    public void MenuRight ()
    {
        objectList[currentObject].SetActive(false);
        currentObject++;

        if (currentObject > objectList.Count - 1)
        {
            currentObject = 0;
        }
        objectList[currentObject].SetActive(true);
    }
  
     
    public void SpwanCurrentObject ()
    {
        Vector3 newRotation = new Vector3(-90, 0, 0);
        Quaternion objectListRotation = objectList[currentObject].transform.rotation;
        if (objectList[currentObject].transform.eulerAngles != Vector3.zero)
        {
            Debug.Log(objectListRotation);
            objectListRotation = Quaternion.Euler(newRotation);
        } 
        Instantiate(objectListPrefabs[currentObject], objectList[currentObject].transform.position,
                    objectListRotation);
    }
}
