﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Instantiate(objectListPrefabs[currentObject], objectList[currentObject].transform.position, objectList[currentObject].transform.rotation);
    }
}
