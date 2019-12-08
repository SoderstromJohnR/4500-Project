using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script prompts the user at the beginning of each episode with instructions on how to
/// accomplish the game's objective.
/// </summary>
public class onLoadPrompter : MonoBehaviour
{
    // Start is called before the first frame update
    // Uses the playerInterruptionActivationController to prompt the user
    void Start()
    {
        // Gets current episode and playerInterruptionActivationController
        Episode currentEpisode = SceneTransitionManager.Instance.currentEpisode;
        playerInterruptionActivatorController controller =
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>();

        // Prompts the user based on the current episode
        switch (currentEpisode)
        {
            case Episode.caving1:
                controller.activateInterrupt(Empty, Skip, 
                    "Place dynamite, go back to the entrance, and press D to detonate.", "Ok!", "Skip");
                break;
            case Episode.caving2:
                controller.activateInterrupt(Empty, Skip,
                    "Do the same thing, but in a more, shall we say, complete way.", "Ok!", "Skip");
                break;
            case Episode.caving3:
                controller.activateInterrupt(Empty, Skip,
                    "Now you can detonate everywhere! See if you can go quicker.", "Right on!", "Skip");
                break;
            case Episode.searching1:
                controller.activateInterrupt(Empty, Skip,
                    "Your buddy lost a chisel. Can you find it?", "Sure!", "No!");
                break;
            case Episode.searching2:
                controller.activateInterrupt(Empty, Skip,
                    "Oh no, now your buddy IS lost! Press S to shout.", "Okay!", "Meh.");
                break;
            default:
                Debug.LogWarning("onLoadPrompter: UNPROMPTED EPISODE");
                break;
        }
    }

    // Passed to do nothing on affirmative click
    void Empty()
    {

    }

    // Skips current episode
    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }
}
