using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class extends the GameStats class to track stats particular to a caving game, including the Indices
/// of the nodes visited and the indices of the rubble piles that have been rigged with dynamite, the
/// number of times the player pressed the dynamite button and some dynamite exploded, and the number
/// of sticks of dynamite that exploded.
/// </summary>
public class CavingGameStats : GameStats
{
    private List<RiggingRecord> riggingRecords; // Records of player activities between detonations
    private int numDetonations;  // The number of times the player detonated dynamite
    private int numExplosions;  // The number of rubble piles that were exploded

    public CavingGameStats()
    {
        // Initalizes the list of rigging records and adds a new one
        riggingRecords = new List<RiggingRecord>();
        riggingRecords.Add(new RiggingRecord());

        // Initializes other stats to 0
        numDetonations = 0;
        numExplosions = 0;

        Debug.LogWarning("New CavingGameStats");
    }


    /* ACCESSORS */

    // Returns the number of times the player has detonated dynamite in this game
    public int getNumDetonations()
    {
        Debug.Log("GameStats - numDetonations: " + numDetonations);
        return numDetonations;
    }

    // Returns the number of sticks of dynamite that have been detonated
    public int getNumExplosions()
    {
        Debug.Log("GameStats - numExplosions: " + numExplosions);
        return numExplosions;
    }

    // Returns a deep copy of the list of rigging records
    public List<RiggingRecord> getRiggingRecords()
    {
        List<RiggingRecord> returnRecords = new List<RiggingRecord>();

        // Copies each record in riggingRecord and adds the copy to returnRecords
        RiggingRecord copy;
        foreach (RiggingRecord record in riggingRecords)
        {
            copy = new RiggingRecord();

            // Copies each member list in each record
            copy.riggedRubbleIndices = record.riggedRubbleIndices.GetRange(0, record.riggedRubbleIndices.Count);
            copy.riggingTraversal = copy.riggingTraversal.GetRange(0, copy.riggingTraversal.Count);

            returnRecords.Add(copy);
        }

        return returnRecords;
    }


    /* MUTATORS */

    // Increments the number of times the player has detonated dynamite
    public void incrementNumDetonations()
    {
        numDetonations++;
        Debug.Log("GameStats - numDetonations: " + numDetonations);

        // Adds the number of charges rigged since last detonation to numExplosions
        numExplosions += riggingRecords[riggingRecords.Count - 1].riggedRubbleIndices.Count;
        Debug.Log("GameStats - numExposions: " + numDetonations);

        riggingRecords.Add(new RiggingRecord());

        wasMutated = true; // Base class indication that a mutator has been called
    }

    /* This method adds the passed index to the visitedNodeIndices set and the traversal list,
     * and the list of nodes visited since the previous detonation (riggingTraversal).
     * It also prints a string representation of caving game statistics. Call this method when 
     * and only when the player has moved to a new node. */
    override public void addVisitedNodeIndex(int index)
    {
        // Adds node to base class traversal and set of visited nodes
        base.addVisitedNodeIndex(index);

        // Adds node to current rigging record traversal
        riggingRecords[riggingRecords.Count - 1].riggingTraversal.Add(index);

        wasMutated = true; // Base class indication that a mutator has been called

        Debug.Log(statsString());
    }

    /* This method adds the passed rubble index to the list of rubble piles rigged w/ explosives
     * Call this method whenever the player adds dynamite to a rubble pile. */
    public void addRiggedRubbleIndex(int index)
    {
        // Adds the index to the current rigging record
        riggingRecords[riggingRecords.Count - 1].riggedRubbleIndices.Add(index);

        wasMutated = true; // Base class indication that a mutator has been called

        Debug.Log(statsString());
    }

    // Returns a string representation of all the player activity in a searching game
    override public string statsString()
    {
        return base.statsString() + cavingStatsString();
    }

    // Returns a string representation of the player activity particular to searching games
    public string cavingStatsString()
    {
        // Adds number of detonations and explosions to return value
        string cavingStats = "\n\n Caving Game Stats:"
                            + "Number of detonations: " + numDetonations.ToString()
                            + "Number of explosions: " + numExplosions.ToString();

        // Adds all the rigging record stats
        int recordNum = 1;
        foreach (RiggingRecord riggingRecord in riggingRecords)
        {
            cavingStats += "\nRigging " + recordNum + ":"
                        + "\nRigged rubble indices: " + Util.intListToString(riggingRecord.riggedRubbleIndices)
                        + "\nRigging traversal: " + Util.intListToString(riggingRecord.riggingTraversal);
        }

        return cavingStats;
    }
}