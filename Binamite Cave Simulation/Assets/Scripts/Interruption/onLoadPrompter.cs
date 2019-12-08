using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onLoadPrompter : MonoBehaviour
{
    Episode currentEpisode;

    // Start is called before the first frame update
    void Start()
    {
        currentEpisode = SceneTransitionManager.Instance.currentEpisode;
        Debug.LogWarning("onLoadPrompter.Start() - Current Episode: " + currentEpisode.sceneName());

        playerInterruptionActivatorController controller =
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>();

        switch (currentEpisode)
        {
            case Episode.caving1:
                controller.activateInterrupt(Empty, Skip, 
                    "Place dynamite, go back, and press D to detonate.", "Ok!", "Skip");
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void Empty()
    {

    }

    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }
}
