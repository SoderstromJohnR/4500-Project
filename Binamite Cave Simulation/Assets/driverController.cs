using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class driverController : MonoBehaviour
{

    GameObject playerInterruptionActivator;

    // Start is called before the first frame update
    void Start()
    {
        playerInterruptionActivator = GameObject.Find("playerInterruptionActivator");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Press Q for testing purposes
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!playerInterruption.activeInHierarchy)
            {
                activateInterrupt(yesMessage, noMessage);
            }
            else if (playerInterruption.activeInHierarchy)
            {
                continueGame();
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>()
                .activateInterrupt(yesMessage, noMessage, "Testing");
        }
    }

    public void yesMessage()
    {
        Debug.Log("driverController - Yes clicked!!!");
    }

    public void noMessage()
    {
        Debug.Log("driverController - No clicked!!!");
    }

}
