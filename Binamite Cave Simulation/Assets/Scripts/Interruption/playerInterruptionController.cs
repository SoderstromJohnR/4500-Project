using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define delegates for methods to call when yes or no are clicked on an interruption
public delegate void OnYesClicked();
public delegate void OnNoClicked();

public class playerInterruptionController : MonoBehaviour
{
    public OnYesClicked onYesClicked; // methods that will be called in yesClicked()
    public OnNoClicked onNoClicked; // methods that will be called in noClicked()

    public void yesClicked()
    {
        onYesClicked();
        Debug.Log("playerInterruptionController - yes clicked");
    }

    public void noClicked()
    {
        onNoClicked();
        Debug.Log("playerInterruptionController - no clicked");
    }
}
