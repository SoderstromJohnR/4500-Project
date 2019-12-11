using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This file defines the behavior of the lost item, causing it to appear in the last un-visited node
/// in the network once the player visits it. 
/// </summary>
public class lostItemController : MonoBehaviour
{
    private HashSet<int> existingNodeIndices; // The set of indices of nodes that exist in the network
    private HashSet<int> visitedNodeIndices; // the set of indices of nodes the player has visited

    private GameObject player; // A reference to the playable character object
    private GameObject[] nodes; // References to all the non-root nodes in the tree

    private bool playerWasMovingLastFrame; // True if the player was moving on the previous update
    private bool playerHasWon; // True if the player has met the victory condition


    // Start is called before the first frame update
    void Start()
    {
        playerWasMovingLastFrame = false;   // Player begins stationary
        playerHasWon = false;               // Player hasn't won yet

        // Initializes index hash sets
        existingNodeIndices = new HashSet<int>();
        visitedNodeIndices = new HashSet<int>();

        // Hides the lost item
        GetComponent<Renderer>().enabled = false;

        // Gets player reference
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player cave index: " + player.GetComponent<playerSC>().getIndex());

        // Gets node references
        GameObject [] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject node in nodes)
        {
            existingNodeIndices.Add(node.GetComponent<nodeStat>().index);
        }
        existingNodeIndices.Add(1); // Adds root index
    }

    // Update is called once per frame
    void Update()
    {
        // Sets playerWasMovingLastFrame to true after the player begins moving
        if (player.GetComponent<playerSC>().checkMoving())
        {
            playerWasMovingLastFrame = true;
        }

        // Adds current player index when the player stops moving
        else if (playerWasMovingLastFrame)
        {
            playerWasMovingLastFrame = false;
            recordCurrentPlayerIndex();
            if (playerHasVisitedAllNodes() && !playerHasWon)
            {
                performVictoryProcedure();
            }
        }
    }

    // Adds the index of the cave the player is currently in to the recorded indices
    void recordCurrentPlayerIndex()
    {
        visitedNodeIndices.Add(player.GetComponent<playerSC>().getIndex());
        Debug.Log("Player index: " + player.GetComponent<playerSC>().getIndex());
    }

    // True if the number of existing node ind
    bool playerHasVisitedAllNodes()
    {
        return existingNodeIndices.Count == visitedNodeIndices.Count;
    }

    // This procedure is intended for when the victory condition of the game has been met
    void performVictoryProcedure()
    {
        Debug.Log("VICTORY!!!!!");
        playerHasWon = true; // Indicates the player has won
        transform.position = player.transform.position; // Moves the item to the player
        GetComponent<Renderer>().enabled = true; // Shows the lost item

        GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>()
            .activateInterrupt(SceneTransitionManager.Instance.loadNextScene, empty, "You did it!", "Next", "Explore");
    }

    // An empty function to pass to the interruption
    void empty() { }

}
