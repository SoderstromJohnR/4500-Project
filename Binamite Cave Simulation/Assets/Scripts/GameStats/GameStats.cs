using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines an encapsulated set of statistics on the players
/// actions over the course of a single game and the nature of the cave network.
/// </summary>

public class GameStats
{
    // Statistics related to general tree traversal
    private List<int> existingNodeIndeces;  // The indeces of nodes in the cave network
    private HashSet<int> visitedNodeIndeces;  // The indeces of nodes the player has visited
    private List<int> traversal;  // The indeces of nodes the player visited in order

    public GameStats()
    {
        existingNodeIndeces = new List<int>();
        visitedNodeIndeces = new HashSet<int>();
        traversal = new List<int>();
    }


    /* ACCESSORS */

    // Returns a copy of the list of the indeces of all the nodes in the cave network
    public List<int> getExistingNodeIndeces()
    {
        return new List<int>(this.existingNodeIndeces);
        Debug.Log("GameStats - existingNodeIndeces: " + this.existingNodeIndeces);
    }

    // Returns a copy of the HashSet of nodes visited by the player in this game
    public HashSet<int> getVisitedNodeIndeces()
    {
        return new HashSet<int>(this.visitedNodeIndeces);
        Debug.Log("GameStats - visitedNodeIndeces: " + this.visitedNodeIndeces);
    }

    // Returns a copy of the list of indecies of nodes in the order they were traversed
    public List<int> getTraversal()
    {
        return new List<int>(this.traversal);
    }
    
    // Returns the number of times the player moved in this game
    public int getNumMoves()
    {
        return traversal.Count;
        Debug.Log("GameStats - numMoves: " + traversal.Count);
    }

    // Returns true of the number of nodes visited equals the number of nodes in the tree
    public bool playerHasVisitedAllNodes()
    {
        int numExisting = existingNodeIndeces.Count;
        int numVisited = visitedNodeIndeces.Count;


        bool hasVisited = numExisting == numVisited && numExisting != 0;
        Debug.Log("GameStats - playerHasVisitedAllNodes:\n existingNodes = " 
            + existingNodeIndeces.Count.ToString() + ", visitedNodes = " + visitedNodeIndeces.Count.ToString());
        Debug.Log("playerHasVisitedAllNodes is " + hasVisited.ToString());

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
            returnValue = -1; // Sentinel indicating no nodes have been visited
        }

        Debug.Log("GameStats.getLastVistedNodeIndex() returning " + returnValue);

        return returnValue;

    }


    /* MUTATORS */

    // Sets the cave network representation existingNodeIndeces to the past list of node indeces
    public void setExistingNodeIndices(List<int> indeces)
    {
        // Alerts if visited nodes haven't been added before modification
        if (visitedNodeIndeces != null && visitedNodeIndeces.Count != 0)
        {
            Debug.Log("GameStats - setExistingNodeIndeces called when visitedNodeIndeces is nonempty");
        }

        this.existingNodeIndeces = indeces.GetRange(0, indeces.Count); // Copies argument list values
    }

    /*  This method adds the passed index to the visitedNodeIndeces set and the traversal list.
        Call this method when and only when the player has moved to a new node. */
    public void addVisitedNodeIndex(int index)
    {
        visitedNodeIndeces.Add(index); // Adds index to HashSet
        traversal.Add(index); // Adds index to list
        Debug.Log("GameStats - Adding index: " + index);
        printList(traversal);
    }

    public void printList(List<int> list)
    {
        string listString = "";

        foreach (int index in list)
        {
            listString += index.ToString() + ", ";
        }

        Debug.Log(listString);
    }
}
