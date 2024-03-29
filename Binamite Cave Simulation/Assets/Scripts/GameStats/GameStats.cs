﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameStats()
    {
        existingNodeIndices = new List<int>();
        visitedNodeIndices = new HashSet<int>();
        traversal = new List<int>();
    }


    /* ACCESSORS */

    // Returns a copy of the list of the Indices of all the nodes in the cave network
    public List<int> getExistingNodeIndices()
    {
        Debug.Log("GameStats - existingNodeIndices: " + this.existingNodeIndices);
        return new List<int>(this.existingNodeIndices);
    }

    // Returns a copy of the HashSet of nodes visited by the player in this game
    public HashSet<int> getVisitedNodeIndices()
    {
        Debug.Log("GameStats - visitedNodeIndices: " + this.visitedNodeIndices);
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
        Debug.Log("GameStats - numMoves: " + traversal.Count);
        return traversal.Count;
    }

    // Returns true of the number of nodes visited equals the number of nodes in the tree
    public bool playerHasVisitedAllNodes()
    {
        int numExisting = existingNodeIndices.Count;
        int numVisited = visitedNodeIndices.Count;


        bool hasVisited = numExisting == numVisited && numExisting != 0;
        Debug.Log("GameStats - playerHasVisitedAllNodes:\n existingNodes = " 
            + existingNodeIndices.Count.ToString() + ", visitedNodes = " + visitedNodeIndices.Count.ToString());
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

    // Sets the cave network representation existingNodeIndices to the past list of node Indices
    public void setExistingNodeIndices(List<int> Indices)
    {
        // Alerts if visited nodes haven't been added before modification
        if (visitedNodeIndices != null && visitedNodeIndices.Count != 0)
        {
            Debug.Log("GameStats - setExistingNodeIndices called when visitedNodeIndices is nonempty");
        }

        this.existingNodeIndices = Indices.GetRange(0, Indices.Count); // Copies argument list values
        Debug.Log("GameStats - Setting existing nodes: \n" + listToString(Indices));
    }

    /*  This method adds the passed index to the visitedNodeIndices set and the traversal list.
        Call this method when and only when the player has moved to a new node. */
    public void addVisitedNodeIndex(int index)
    {
        visitedNodeIndices.Add(index); // Adds index to HashSet
        traversal.Add(index); // Adds index to list
        Debug.Log("GameStats - Adding index: " + index
            + "\n Traversal: " + listToString(traversal));
    }

    private static string listToString(List<int> list)
    {
        string listString = "";

        foreach (int index in list)
        {
            listString += index.ToString() + ", ";
        }
        listString.Remove(listString.Length - 1, 1);

        return listString;
    }
}
