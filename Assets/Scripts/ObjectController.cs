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

	[Header( "Level" )]
	private int maxCountForLevel1;
	private int maxCountForLevel2;
	private int maxCountForLevel3;
	private int maxCountForLevel4;

	public void InitializeMaxCount (int level1, int level2, int level3, int level4) {
		maxCountForLevel1 = level1;
		maxCountForLevel2 = level2;
		maxCountForLevel3 = level3;
		maxCountForLevel4 = level4;
	}

	public void SpawnObject (GameObject go, Transform tf)
    {
        if (GetMaxCount() != 0)
        {
			Instantiate(go, tf.position, tf.rotation);
            GetMaxCount(1);
        }
    }

    int GetMaxCount (int amountToDecrease = 0)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                maxCountForLevel1 -= amountToDecrease;
                return maxCountForLevel1;
            case "Level2":
                maxCountForLevel2 -= amountToDecrease;
                return maxCountForLevel2;
            case "Level3":
                maxCountForLevel3 -= amountToDecrease;
                return maxCountForLevel3;
            case "Level4":
                maxCountForLevel4 -= amountToDecrease;
                return maxCountForLevel4;
            default:
                return -1;
        }
    }
}
