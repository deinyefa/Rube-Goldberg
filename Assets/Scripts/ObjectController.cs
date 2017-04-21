/*
   * Level 1: Spawn as many as you like
   * Level 2: Spawn 3 of each object
   * Level 3: Spawn 3 of one object and 1 of the others
   * Level 4: Spawn 1 of each object 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectController : MonoBehaviour {

    public int maxCountForLevel1;
    public int maxCountForLevel2;
    public int maxCountForLevel3;
    public int maxCountForLevel4;

    private ObjectMenuManager objectMenuManager;

    private void Start()
    {
        objectMenuManager = GameObject.FindObjectOfType<ObjectMenuManager>();
    }

    public void SpawnObject ()
    {
        if (GetMaxCount() != 0)
        {
            Instantiate(objectMenuManager.objectListPrefabs[objectMenuManager.currentObject],
                objectMenuManager.objectList[objectMenuManager.currentObject].transform.position,
              objectMenuManager.objectList[objectMenuManager.currentObject].transform.rotation);
            GetMaxCount(1);
        }
    }

    int GetMaxCount (int amountToDecrease = 0)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                maxCountForLevel1 = maxCountForLevel1 - amountToDecrease;
                return maxCountForLevel1;
            case "Level2":
                maxCountForLevel2 = maxCountForLevel2 - amountToDecrease;
                return maxCountForLevel2;
            case "Level3":
                maxCountForLevel3 = maxCountForLevel3 - amountToDecrease;
                return maxCountForLevel3;
            case "Level4":
                maxCountForLevel4 = maxCountForLevel4 - amountToDecrease;
                return maxCountForLevel4;
            default:
                return -1;
        }
    }
}
