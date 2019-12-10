using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class extends the GameStats class, allowing tracking of the number
/// of times the player shouted.
/// </summary>

public class SearchingGameStats : GameStats
{   
    private int numShouts; // The number of times the player has shouted

    // Initializes numShouts to 0
    public SearchingGameStats()
    {
        numShouts = 0;
        Debug.LogWarning("New SearchingGameStats");
    }

    // Returns the number of times the player has shouted this game
    public int getNumShouts()
    {
        return numShouts;
    }

    // Increments the number of times the player has shouted
    public void incrementNumShouts()
    {
        Debug.Log("GameStats - numShouts: " + numShouts);
        numShouts++;

        wasMutated = true; // Base class indication that a mutator has been called
    }

    /* This method adds the passed index to the visitedNodeIndices set and the traversal list
       by calling the overridden method in the base class. It also prints a string representation
       of searching game statistics. Call this method when and only when the player has moved
       to a new node. */
    override public void addVisitedNodeIndex(int index)
    {
        base.addVisitedNodeIndex(index);

        Debug.Log(statsString());
    }

    // Returns a string representation of all the player activity in a searching game
    override public string statsString()
    {
        return base.statsString() + searchingStatsString();
    }

    // Returns the player activity particular to searching games
    public string searchingStatsString()
    {
        return "\n\nSearching Game Stats:"
             + "Number of shouts: " + numShouts.ToString();
    } 
}
