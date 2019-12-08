using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro; //The TextMeshPro library

// Define delegates for methods to call when yes or no are clicked on an interruption
public delegate void OnYesClicked();
public delegate void OnNoClicked();

/// <summary>
/// playerInterruptionController.cs defines the behavior of a UI element that interrupts
/// the player. It should be attatched to the playerInterruption prefab, which is designed
/// to be activated by the script playerInterruptionActivatorController. The prefab has
/// two buttons and a text area containing text that can be set with setMessage, 
/// setAffirmativeText, and setNegativeText, respectively.
/// 
/// The methods yesClicked and noClicked execute whichever methods were added to the 
/// onYesClicked and onNoClicked delegates. They're intended to be delegates of the left
/// and right buttons, respectively.
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
