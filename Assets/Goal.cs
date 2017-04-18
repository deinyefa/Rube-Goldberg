using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public GameObject[] collectables;

    void Update () {
        LoadNewLevel();
	}

    void LoadNewLevel()
    {
        if (AreStarsInActive())
        {
            Debug.Log("loading Level 2");
            SteamVR_LoadLevel.Begin("nextLevel");
        }
    }

    bool AreStarsInActive()
    {
        foreach (GameObject collectable in collectables)
        {
            if (collectable.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
