using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class defines an encapsulated set of statistics on the players actions over the course 
/// of a single episode, as well as a representation of the cave network as a set of indices.
/// </summary>

public class GameStats
{
    // Statistics related to general tree traversal
    private List<int> existingNodeIndices;  // The Indices of nodes in the cave network
    private HashSet<int> visitedNodeIndices;  // The Indices of nodes the player has visited
    private List<int> traversal;  // The Indices of nodes the player visited in order

    protected bool wasMutated = false; // Indicates whether mutators have been called

    public GameStats()
    {
        existingNodeIndices = new List<int>();
        visitedNodeIndices = new HashSet<int>();
        traversal = new List<int>();

        // Represents that the player has visited the root node
        visitedNodeIndices.Add(1);
        traversal.Add(1);
    }


    /* ACCESSORS */

    // Returns a copy of the list of the Indices of all the nodes in the cave network
    public List<int> getExistingNodeIndices()
    {
        return new List<int>(this.existingNodeIndices);
    }

    // Returns a copy of the HashSet of nodes visited by the player in this game
    public HashSet<int> getVisitedNodeIndices()
    {
        return new HashSet<int>(this.visitedNodeIndices);
    }

    // Returns a copy of the list of indecies of nodes in the order they were traversed
    public List<int> getTraversal()
    {
        return new List<int>(this.traversal);
    }
    
    // Returns the number of times the player moved in this game
    public int getNumMoves()
    {
        return traversal.Count - 1;
    }

    // Returns the number of caves in the cave system
    public int getNumCaves()
    {
        return existingNodeIndices.Count;
    }

    // Returns true of the number of nodes visited equals the number of nodes in the tree
    public bool playerHasVisitedAllNodes()
    {
        int numExisting = existingNodeIndices.Count;
        int numVisited = visitedNodeIndices.Count;


        bool hasVisited = numExisting == numVisited && numExisting != 0;

        return hasVisited;
    }

    // Returns the index of the last node the player has visited
    public int getLastVisitedNodeIndex()
    {
        int returnValue;
        if (traversal.Count > 0)
        {
            returnValue = traversal[traversal.Count - 1];
        }
        else
        {
            returnValue = -1; // Sentinel indicating the player hasn't moved
        }

        Debug.Log("GameStats.getLastVistedNodeIndex() returning " + returnValue
            + "\n" + statsString());

        return returnValue;

    }

    // Can be overridden in subclasses to augment a string representation of player activity
    virtual public string statsString()
    {
        string str = "Traversal: " + Util.intListToString(traversal)
                + "\nExisting node indices: " + Util.intListToString(existingNodeIndices)
                + "\nVisited node indices: " + Util.intListToString(visitedNodeIndices.ToList())
                + "\nNumber of existing nodes: " + existingNodeIndices.Count
                + "\nNumber of visited nodes: " + visitedNodeIndices.Count
                + "\nNumber of moves: " + (traversal.Count - 1)
                + "\nThat the player has visited all nodes is " + playerHasVisitedAllNodes().ToString();

        return str;
    }

    // Returns true if a mutator has been called on a given instance
    public bool hasBeenMutated()
    {
        return wasMutated;
    }

    /* MUTATORS */

    // Sets the cave network representation existingNodeIndices to the past list of node Indices
    public void setExistingNodeIndices(List<int> Indices)
    {
        // Alerts if visited nodes haven't been added before modification
        if (visitedNodeIndices != null && visitedNodeIndices.Count != 0)
        {
            Debug.LogWarning("GameStats - setExistingNodeIndices called when visitedNodeIndices is nonempty");
        }

        this.existingNodeIndices = Indices.GetRange(0, Indices.Count); // Copies argument list values
        wasMutated = true;
        //Debug.Log("GameStats - Setting existing nodes\n" + statsString());
    }

    /*  This method adds the passed index to the visitedNodeIndices set and the traversal list.
        Call this method when and only when the player has moved to a new node. */
    virtual public void addVisitedNodeIndex(int index)
    {
        visitedNodeIndices.Add(index); // Adds index to HashSet
        traversal.Add(index); // Adds index to list
        wasMutated = true;
        // Debug.Log("GameStats - Adding index: " + index + "\n" + statsString());
    }

    

}
