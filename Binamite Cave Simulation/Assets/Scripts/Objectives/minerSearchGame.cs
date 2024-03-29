﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minerSearchGame : MonoBehaviour
{
    private GameObject minerShout;
    private GameObject minerThanks;
    private GameObject player;
    
    private float distance;
    private Vector3 minerShoutPosition;
    private int caveIndex; // The index of the cave the miner is in
    private GameObject root;

    private bool minerFound;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
        //Set the GameObject's Color to black
        nodeImage.color = Color.black;

        GetComponent<SpriteRenderer>().sortingLayerName = "OtherMiner";

        minerShout = Resources.Load<GameObject>("minerShout");
        minerThanks = Resources.Load<GameObject>("minerThanks");
        root = GameObject.Find("Root Node");

        player = GameObject.Find("player");
        Vector3 nodeSize = GetComponentInParent<Renderer>().bounds.size / 1.5f;
        distance = nodeSize.x * nodeSize.x + nodeSize.y * nodeSize.y;
        distance = Mathf.Pow(distance, 0.5f);

        //Get cave index for miner from its parent cave.
        caveIndex = GetComponentInParent<nodeStat>().getIndex();
        Debug.Log("Miner index: " + caveIndex.ToString());

        minerFound = false;
    }

    public int getCaveIndex()
    {
        return caveIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //Get current location of the player every time they shout
        int playerIndex = player.GetComponent<playerSC>().getIndex();

        //Use Shout key, set to S for now, check if player is moving or not
        if (Input.GetKeyDown(KeyCode.S) && !player.GetComponent<playerSC>().checkMoving())
        {
            //Check if we are on the left or right subtree of player
            bool isLeft = false;
            int checkIndex = caveIndex;
            
            //Check through parent caves up to root or until it finds the player
            while (checkIndex != 0 && checkIndex != playerIndex)
            {
                //Track if the child we moved from is the left or right child of the parent
                if ((checkIndex % 2) == 0)
                {
                    isLeft = true;
                }
                else
                {
                    isLeft = false;
                }
                //Move to next parent node
                checkIndex /= 2;
            }

            //If previous loop completed without finding the player, they are in the wrong subtree
            if (checkIndex != playerIndex)
            {
                //Checks if the parent node of the player is the root, which doesn't have an index to find
                //through findObject(int index)
                if (playerIndex / 2 == 1)
                {
                    minerShoutPosition = root.transform.position;
                }
                else
                {
                    minerShoutPosition = root.GetComponent<johnRootController>().findObject(playerIndex / 2).transform.position;
                }
                minerShoutPosition -= player.transform.position;
                minerShoutPosition = minerShoutPosition.normalized * distance;
                minerShoutPosition += player.transform.position;
                Debug.Log("I'm behind you!");
                Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
            }
            //If the player was found and they aren't already in the same cave as the miner,
            //run this check.
            else if (caveIndex != playerIndex)
            {
                //If the last node checked was the left child, mark miner on left and place
                //speech bubble appropriately
                if (isLeft)
                {
                    minerShoutPosition = root.GetComponent<johnRootController>().findObject(playerIndex * 2).transform.position;
                    minerShoutPosition -= player.transform.position;
                    minerShoutPosition = minerShoutPosition.normalized * distance;
                    minerShoutPosition += player.transform.position;
                    Debug.Log("I'm on the left!");
                    Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
                }
                //This goes off if the last node checked was the right child
                else
                {
                    minerShoutPosition = root.GetComponent<johnRootController>().findObject(playerIndex * 2 + 1).transform.position;
                    minerShoutPosition -= player.transform.position;
                    minerShoutPosition = minerShoutPosition.normalized * distance;
                    minerShoutPosition += player.transform.position;
                    Debug.Log("I'm on the right!");
                    Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
                }

                //Increment number of shouts made
                player.GetComponent<playerSC>().incMinerShout();
            }
        }

        // Displays minerThanks object if the miner has been found for the first time
        if (!minerFound && player.GetComponent<playerSC>().checkMinerFound())
        {
            minerShoutPosition = transform.parent.position + new Vector3(0, distance, 0);
            Debug.Log("You found me! Thanks!");
            Instantiate(minerThanks, minerShoutPosition, Quaternion.identity);

            minerFound = true;
        }

        imVisible();
    }
    public void imVisible()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 3)
        {
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to green
            nodeImage.color = Color.white;
        }
        
    }
}
