using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a controller for an empty game object which is designed to activate
/// an object with playerInterruptionController attached when activateInterrupt is called.
/// 
/// Methods that conform to the OnYesClicked and OnNoClicked protocols defined in
/// playerInterruptionController.cs can then be passed to it through the method activateInterrupt,
/// defined in this file.
/// 
/// To configure this script to activate an object:
///     1. Drag the playerInterruptionActivator onto the hierarchy pane.
///     2. Drag the object to be instantiated by this script into the hierarchy pane.
///        (this project includes a playerInterruption prefab in Assets/Prefabs/Interruption)
///     3. Highlight the playerInterruptionActivator.
///     4. Drag playerInterruption from the hierarchy to the Player Interruption area in the
///        Player Interruption Activator Controller (Script) area in the inspector.
/// 
/// To activate an object that has been configured as above, the following call can be used directly:
///     
///        GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>()
///            .activateInterrupt(onYesClicked, onNoClicked, "Message");
///             
/// where onYesClicked and onNoClicked are methods that conform to the aformentioned OnYesClicked and OnNoClick
/// protocols in the file playerInterruptionController.cs.
/// </summary>

public class playerInterruptionActivatorController : MonoBehaviour
{
    [SerializeField] private GameObject playerInterruption;

    // Start is called before the first frame update
    void Start()
    {
        // Disables the interruption object by default
        playerInterruption.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Press Q to activate the playerInterruption for testing purposes
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
    }

    // Prints a yes message to console
    public void yesMessage()
    {
        Debug.Log("playerInterruptionActivatorController - Yes!");
    }

    // Prints a no message to console
    public void noMessage()
    {
        Debug.Log("playerInterruptionActivatorController - No!");
    }

    /* Sets the assigned playerInterruption game object to active and passes methods yesClicked 
     * and noClicked that conform to the delegates OnYesClicked and OnNoClicked defined in 
     * playerInterruptionController.cs */
    public void activateInterrupt(OnYesClicked yesClicked, OnNoClicked noClicked, string message = null)
    {
        if (!playerInterruption.activeInHierarchy)
        {
            Debug.Log("playerInterruptionActivatorController - Interrupt Activated!");

            // Activates interruption
            playerInterruption.SetActive(true);

            // Overwrites delegates for yes and no buttons
            playerInterruption.GetComponent<playerInterruptionController>().onYesClicked = yesClicked;
            playerInterruption.GetComponent<playerInterruptionController>().onNoClicked = noClicked;

            // Adds continueGame to the button delegates
            playerInterruption.GetComponent<playerInterruptionController>().onYesClicked += continueGame;
            playerInterruption.GetComponent<playerInterruptionController>().onNoClicked += continueGame;

            // This "stops time," by disabling scripts that still work while timescale is set to 1
            Time.timeScale = 0;
        }
    }

    // This method returns the timescale to 1 and movement resumes
    private void continueGame()
    {
        Debug.Log("playerInterruptionActivatorController - Game Continued");
        playerInterruption.SetActive(false);

        // Enables scripts that require a non-zero timescale
        Time.timeScale = 1;
    }
}
