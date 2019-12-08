using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a record of player activity between detonation events, including the Indices
/// of the nodes visited and the indecies of the rubble piles that have been rigged with dynamite.
/// </summary>
public class RiggingRecord
{
    public List<int> riggedRubbleIndices; // The indecies of rubble piles rigged
    public List<int> riggingTraversal; // Indecies of visited nodes since previous detonation

    public RiggingRecord()
    {
        riggedRubbleIndices = new List<int>();
        riggingTraversal = new List<int>();
    }
}
