using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a controller that activates a prompt that interrupts the player to ask
/// a question. When the player clicks yes or no, the corresponding method that was passed to
/// it will be called.
/// 
/// Methods that conform to the OnYesClicked and OnNoClicked protocols defined in
/// playerInterruptionController.cs can then be passed to it through the method activateInterrupt,
/// defined in this file.
/// 
/// To configure this script to activate an object that interrupts the game:
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
/// where onYesClicked and onNoClicked are methods that conform to the OnYesClicked and OnNoClick
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

    /* Sets the assigned playerInterruption game object to active, sets method arguments as delegates,
     * and stops time. */
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
