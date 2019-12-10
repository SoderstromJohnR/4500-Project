using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a controller that activates a prompt that interrupts the player to ask
/// a question. When the player clicks yes or no, the corresponding method that was passed to
/// it will be called.
/// 
/// Methods that conform to the OnLeftButtonClicked and OnRightButtonClicked protocols defined in
/// playerInterruptionController.cs can then be passed to it through the method activateInterrupt,
/// defined in this file.
/// 
/// The playerInterruptionActivator is part of the interruptSystem prefab and activates the 
/// playerInterruption prefab contained therein. 
/// </summary>

public class playerInterruptionActivatorController : MonoBehaviour
{
    public delegate void onInterruptionStart();

    // Set as playerInterruption prefab in the editor
    [SerializeField] private GameObject playerInterruption;

    // Start is called before the first frame update
    void Start()
    {
        playerInterruption.SetActive(false);

        // Prompts the user with instructions
        promptWithInstructions();
    }

    /* Sets the assigned playerInterruption game object to active, sets method arguments as delegates,
     * stops time, and passes optional arguments representing new message text, left button text, and
     * right button text to the newly activated interruption prefab. */
    public void activateInterrupt(OnLeftButtonClicked leftButtonClicked, OnRightButtonClicked rightButtonClicked,
        string message = null, string leftText = null, string rightText = null)
    {
        if (!playerInterruption.activeInHierarchy)
        {
            playerInterruptionController interruptionController =
                playerInterruption.GetComponent<playerInterruptionController>();

            // Activates interruption
            playerInterruption.SetActive(true);

            // Overwrites delegates for yes and no buttons
            interruptionController.onLeftButtonClicked = leftButtonClicked;
            interruptionController.onRightButtonClicked = rightButtonClicked;

            // Adds continueGame to the button delegates
            interruptionController.onLeftButtonClicked += continueGame;
            interruptionController.onRightButtonClicked += continueGame;

            // Sets text if arguments are not default
            if (message != null)
            {
                interruptionController.setMessage(message);
            }

            if (leftText != null)
            {
                interruptionController.setLeftButtonText(leftText);
            }

            if (rightText != null)
            {
                interruptionController.setRightButtonText(rightText);
            }

            // This "stops time" by disabling scripts that still work while timescale is set to 1
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

    // Activates the playerInterruption with an episode-specific message instructing the player
    void promptWithInstructions()
    {
        Episode currentEpisode = SceneTransitionManager.Instance.currentEpisode;

        switch (currentEpisode)
        {
            case Episode.caving1:
                activateInterrupt(Empty, Skip,
                    "Place dynamite, go back to the entrance, and press D to detonate.", "Ok!", "Skip");
                break;
            case Episode.caving2:
                activateInterrupt(Empty, Skip,
                    "Do the same thing, but in a more, shall we say, complete way.", "Ok!", "Skip");
                break;
            case Episode.caving3:
                activateInterrupt(Empty, Skip,
                    "Now you can detonate everywhere! See if you can go quicker.", "Right on!", "Skip");
                break;
            case Episode.searching1:
                activateInterrupt(Empty, Skip,
                    "Your buddy lost a chisel. Can you find it?", "Sure!", "No!");
                break;
            case Episode.searching2:
                activateInterrupt(Empty, Skip,
                    "Oh no, now your buddy IS lost! Press S to shout.", "Okay!", "Meh.");
                break;
            default:
                break;
        }
    }

    // An empty method to pass to the activateInterrupt method
    void Empty() { }

    // Loads the next scene using SceneTransitionManager
    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }
}