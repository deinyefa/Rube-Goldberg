using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour {

    public void LoadLevelOne ()
    {
        SteamVR_LoadLevel.Begin("Level1");
    }

    public void LoadStartScreen ()
    {
        SteamVR_LoadLevel.Begin("StartScreen");
    }
}
