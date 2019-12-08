using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class extends the GameStats class to track stats particular to a caving game, including the Indices
/// of the nodes visited and the indecies of the rubble piles that have been rigged with dynamite, the
/// number of times the player pressed the dynamite button and some dynamite exploded, and the number
/// of sticks of dynamite that were exploded.
/// </summary>
public class CavingGameStats : GameStats
{
    private List<RiggingRecord> riggingRecords; // Records of player activities between detonations
    private int numDetonations;  // The number of times the player detonated dynamite
    private int numExplosions;  // The number of rubble piles that were exploded

    public CavingGameStats()
    {
        riggingRecords = new List<RiggingRecord>();
        riggingRecords.Add(new RiggingRecord());
        numDetonations = 0;
        numExplosions = 0;


        Debug.LogWarning("New CavingGameStats");
    }


    /* ACCESSORS */

    // Returns the number of times the player has detonated dynamite in this game
    public int getNumDetonations()
    {
        return numDetonations;
        Debug.Log("GameStats - numDetonations: " + numDetonations);
    }

    // Returns the number of sticks of dynamite that have been detonated
    public int getNumExplosions()
    {
        return numExplosions;
        Debug.Log("GameStats - numExplosions: " + numExplosions);
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

        riggingRecords.Add(new RiggingRecord());
    }

    /*  This method adds the passed index to the visitedNodeIndices set and the traversal list,
     *  and the list of nodes visited since the previous detonation (riggingTraversal).
     *  Call this method when and only when the player has moved to a new node. */
    public void addVisitedNodeIndex(int index)
    {
        // Adds node to base class traversal
        base.addVisitedNodeIndex(index);

        // Adds node to current rigging record traversal
        riggingRecords[riggingRecords.Count - 1].riggingTraversal.Add(index);
    }

    /* This method adds the passed rubble index to the list of rubble piles rigged w/ explosives
     * Call this method whenever the player adds dynamite to a rubble pile. */
    public void addRiggedRubbleIndex(int index)
    {
        // Adds the index to the current rigging record
        riggingRecords[riggingRecords.Count - 1].riggedRubbleIndices.Add(index);
    }
}
