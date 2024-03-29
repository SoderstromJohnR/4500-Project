﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a controller that activates a quiz display UI component.
/// </summary>

public class quizDisplayActivatorController : MonoBehaviour
{
    // Set as quizDisplay prefab in the editor
    [SerializeField] private GameObject quizDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Disables the interruption object by default
        quizDisplay.SetActive(false);
    }

    /* Sets the assigned playerInterruption game object to active, sets method arguments as delegates,
     * stops time, and passes optional arguments representing new message text, left button text, and
     * right button text to the newly activated interruption prefab. 
    public void activateInterrupt(OnYesClicked yesClicked, OnNoClicked noClicked,
        string message = null, string affirmativeText = null, string negativeText = null)
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

            // Sets text if arguments are not default
            if (message != null)
            {
                playerInterruption.GetComponent<playerInterruptionController>().setMessage(message);
            }

            if (affirmativeText != null)
            {
                playerInterruption.GetComponent<playerInterruptionController>().setAffirmativeText(affirmativeText);
            }

            if (negativeText != null)
            {
                playerInterruption.GetComponent<playerInterruptionController>().setNegativeText(negativeText);
            }

            // This "stops time" by disabling scripts that still work while timescale is set to 1
            Time.timeScale = 0;
        }
    }*/

    // This method returns the timescale to 1 and movement resumes
    private void continueGame()
    {
        Debug.Log("quizDisplayActivatorController - Game Continued");
        quizDisplay.SetActive(false);

        // Enables scripts that require a non-zero timescale
        Time.timeScale = 1;
    }

    /* // Activates the playerInterruption with an episode-specific message instructing the player
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
    }*/

    // An empty method to pass to the showQuiz method
    void Empty() { }

    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }
}