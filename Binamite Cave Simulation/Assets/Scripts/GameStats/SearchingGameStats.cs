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

    public SearchingGameStats()
    {
        numShouts = 0;
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
    }
}
