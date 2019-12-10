using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro; //The TextMeshPro library

// Define delegates for methods to call when left or left are clicked on an interruption
public delegate void OnLeftButtonClicked();
public delegate void OnRightButtonClicked();

/// <summary>
/// This script defines the behavior of a UI element that interrupts
/// the player. It should be attached to the playerInterruption prefab, which is designed
/// to be activated by the script playerInterruptionActivatorController. The prefab has
/// two buttons and a text area containing text that can be set with setMessage, 
/// setleftButtonText, and setleftButtonText, respectively.
/// 
/// The methods leftClicked and leftClicked execute whichever methods were added to the 
/// OnLeftButtonClicked and OnRightButtonClicked fields. Any such methods referenced in OnLeftButtonClicked 
/// and OnRightButtonClicked will be called when the player clicks the left or right buttons, 
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
///            .activateInterrupt(OnLeftButtonClicked, OnRightButtonClicked, "Message");
///             
/// where OnLeftButtonClicked and OnRightButtonClicked are methods that conform to the OnLeftButtonClicked and 
/// OnRightClick protocols in the file playerInterruptionController.cs.
/// 
/// </summary>

public class playerInterruptionController : MonoBehaviour
{
    // TextMeshPro text objects to be set in inspector
    public GameObject messageText; // The message to display
    public GameObject leftButtonText; // The text on the left button
    public GameObject rightButtonText; // The text on the right button

    public OnLeftButtonClicked onLeftButtonClicked; // Methods that will be called in leftClicked()
    public OnRightButtonClicked onRightButtonClicked; // Methods that will be called in leftClicked()

    // Calls delegate methods associated with the affirmative button
    public void leftClicked()
    {
        onLeftButtonClicked();
        Debug.Log("playerInterruptionController - left clicked");
    }

    // Calls delegate methods associated with the negative button
    public void rightClicked()
    {
        onRightButtonClicked();
        Debug.Log("playerInterruptionController - right clicked");
    }

    // Sets the message text to be displayed
    public void setMessage(string message)
    {
        messageText.GetComponent<TextMeshProUGUI>().SetText(message);
    }

    // Sets the text on the left button
    public void setLeftButtonText(string leftText)
    {
        leftButtonText.GetComponent<TextMeshProUGUI>().SetText(leftText);
    }

    // Sets the text on the right 
    public void setRightButtonText(string rightText)
    {
        rightButtonText.GetComponent<TextMeshProUGUI>().SetText(rightText);
    }
}
