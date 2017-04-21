using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Goal : MonoBehaviour {

    public GameObject[] collectables;
	public GameObject goal;
    

    void Update () {
        LoadNewLevel();
	}

    void LoadNewLevel()
    {
        if (AreStarsAndGoalInActive())
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1":
                    SteamVR_LoadLevel.Begin("level2");
                    break;
                case "Level2":
                    SteamVR_LoadLevel.Begin("level3");
                    break;
                case "Level3":
                    SteamVR_LoadLevel.Begin("level3");
                    break;
                case "Level4":
                    SteamVR_LoadLevel.Begin("WinScreen");
                    break;
                default: Debug.Log("StartScreen");
                    break;
            }
        }
    }

    bool AreStarsAndGoalInActive()
    {
        foreach (GameObject collectable in collectables)
        {
			if (collectable.gameObject.activeInHierarchy && goal.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
