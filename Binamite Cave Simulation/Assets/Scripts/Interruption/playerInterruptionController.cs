using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro; //The TextMeshPro library

// Define delegates for methods to call when yes or no are clicked on an interruption
public delegate void OnYesClicked();
public delegate void OnNoClicked();

/// <summary>
/// This script defines the behavior of a UI element that interrupts
/// the player. It should be attached to the playerInterruption prefab, which is designed
/// to be activated by the script playerInterruptionActivatorController. The prefab has
/// two buttons and a text area containing text that can be set with setMessage, 
/// setAffirmativeText, and setNegativeText, respectively.
/// 
/// The methods yesClicked and noClicked execute whichever methods were added to the 
/// onYesClicked and onNoClicked fields. Any such methods referenced in onYesClicked 
/// and onNoClicked will be called when the player clicks the left or right buttons, 
/// respectively.
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
/// 
/// </summary>

public class playerInterruptionController : MonoBehaviour
{
    // TextMeshPro text objects to be set in inspector
    public GameObject messageText; // The message to display
    public GameObject affirmativeText; // The text on the yes button
    public GameObject negativeText; // The text on the no button

    public OnYesClicked onYesClicked; // Methods that will be called in yesClicked()
    public OnNoClicked onNoClicked; // Methods that will be called in noClicked()

    // Calls delegate methods associated with the affirmative button
    public void yesClicked()
    {
        onYesClicked();
        Debug.Log("playerInterruptionController - yes clicked");
    }

    // Calls delegate methods associated with the negative button
    public void noClicked()
    {
        onNoClicked();
        Debug.Log("playerInterruptionController - no clicked");
    }

    // Sets the message text to be displayed
    public void setMessage(string message)
    {
        messageText.GetComponent<TextMeshProUGUI>().SetText(message);
    }

    // Sets the text on what would otherwise be the yes button
    public void setAffirmativeText(string affirmation)
    {
        affirmativeText.GetComponent<TextMeshProUGUI>().SetText(affirmation);
    }

    // Sets the text on what would otherwise be the no buton
    public void setNegativeText(string negation)
    {
        negativeText.GetComponent<TextMeshProUGUI>().SetText(negation);
    }
}
