using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintVariables : MonoBehaviour {
    
    private bool hasTeleported;
    private bool ballHasBeenLifted;
    private bool hasCompletedLeftControllerTutorial;
    private bool hasCompletedRightControllerTutorial;

    public bool HasTeleport
    {
        get
        {
            return hasTeleported;
        }
        set
        {
            hasTeleported = true;
        }
    }

    public bool BallHasBeenLifted
    {
        get
        {
            return ballHasBeenLifted;
        }
        set
        {
            ballHasBeenLifted = true;
        }
    }

    public bool LeftControllerTutorialCompleted
    {
        get
        {
            return hasCompletedLeftControllerTutorial;
        } 
        set
        {
            hasCompletedLeftControllerTutorial = true;
        }
    }

    public bool RightControllerTutorialConpleted
    {
        get
        {
            return hasCompletedRightControllerTutorial;
        }
        set
        {
            hasCompletedRightControllerTutorial = true; 
        }
    }
}
