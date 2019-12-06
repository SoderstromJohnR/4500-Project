using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSC : MonoBehaviour
{
    public float playerSpeed;
    public GameObject camera;
    public GameObject root;

    private int caveIndex;
    private int currentCaveIndex;
    private float playerActualSpeed;
    private float targetDistance;
    private float elapsedTime;
    private float expectedTime;

    private bool isMoving = false;
    private bool foundMiner = false;
    private bool clearDebris = false;
    private bool cameraFollow = true;

    private Vector3 targetPosition;
    private Vector3 clickPosition;
    private Vector3 storePosition;

    private bool detonation;
    [SerializeField] private int numCaveMoves;
    [SerializeField] private int numExplosions;
    [SerializeField] private int numDetonations;
    [SerializeField] private int numMinerShouts;

    /* Ensure the player and any initial stats are
     * set at the start of the game.
     */
    void Start()
    {
        //Set the target position immediately to the player's starting location
        targetPosition = transform.position;
        caveIndex = 1;

        //Initialize tracking stats which can be used to determine scores or points
        //at the end of an episode
        numCaveMoves = 0;
        numMinerShouts = 0;
        numExplosions = 0;
        numDetonations = 0;
        detonation = false;
    }

    /* Update is called once per frame.
     * Handles a variety of possible player actions including
     * retrieving coordinates for movement and facing the player.
     */
    void Update()
    {
        //Test for a mouse click and make sure the player is not currently moving
        //or placing debris.
        if (!clearDebris && !isMoving && Input.GetMouseButtonDown(0))
        {
            //Get world coordinates of mouse input
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 click2D = new Vector2(clickPosition.x, clickPosition.y);

            //Check for click on object, go to center of object instead of mouse click
            //hitAll will include any gameObjects with a collider at the click's location
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit2D[] hitAll = Physics2D.RaycastAll(click2D, Vector2.zero);

            //Skip movement checks if no objects were hit
            if (hitAll.Length > 0)
            {
                //The camera should follow in most situations
                cameraFollow = true;
                //If we will move anywhere, store our current position first
                //Used mostly for clearing debris where the player returns to the center of its cave
                storePosition = transform.position;

                //A solution I'm a bit happier with. Unlike the Raycast2D object, GameObject can be set to null at the start
                //Everything in the list had a collision, so there is no need to check for that. The tempPriority gives us
                //an easy way to stop from clicking on something unintended when objects overlap.
                GameObject hit = null;
                int tempPriority = 99;      //Set a high temporary priority as we determine which gameobjects are more important
                foreach (RaycastHit2D temp in hitAll)
                {
                    //Debris is the highest priority. The player can only go to it if it's in the same cave,
                    //though the player spazzes out a little if they click on one in another.
                    if (temp.collider.gameObject.tag == "Debris")
                    {
                        Debug.Log("Selecting Debris");
                        tempPriority = 1;
                        hit = temp.collider.gameObject;
                    }
                    //The Cave Exit should only be available if any debris over it is clear.
                    else if (tempPriority > 1 && temp.collider.gameObject.tag == "CaveExit")
                    {
                        //Only cave exits on the current cave should work
                        if (temp.collider.gameObject.GetComponent<caveExitController>().originIndex == caveIndex)
                        {
                            Debug.Log("Selecting CaveExit on current cave");
                            tempPriority = 2;
                            hit = temp.collider.gameObject;
                        }
                    }
                    //The caves/nodes have the largest area. If a player clicks near the higher priorities, they most likely
                    //want to use those and not the cave itself.
                    else if (tempPriority > 2 && (temp.collider.gameObject.tag == "Node" || temp.collider.gameObject.name == "Root Node"))
                    {
                        Debug.Log("Selecting Cave");
                        tempPriority = 3;
                        hit = temp.collider.gameObject;
                    }
                }

                //Sanity check in case no legal objects were found in the list
                if (hit)
                {
                    //Use this if the player clicks on the root node/entrance
                    if (hit.name == "Root Node")
                    {
                        if (CaveIsReachable(1))
                        {
                            root.GetComponent<johnRootController>().addVisitedIndex(1);
                            playerActualSpeed = playerSpeed;
                            if (caveIndex != 1) isMoving = true;
                            caveIndex = 1;
                            Debug.Log("Going to center of entrance instead");
                            targetPosition = hit.transform.position;
                            //Send new index to camera
                            setExpectedTime();
                            camera.GetComponent<cameraController>().changePlayerIndex(caveIndex, expectedTime);
                            // Records cave move
                            numCaveMoves += 1;
                            SceneTransitionManager.Instance.currentGameStats.addVisitedNodeIndex(1);
                        }
                    }
                    //Use this if the player clicked on a cave other than the entrance
                    else if (hit.name == "Node(Clone)")
                    {
                        int hitIndex = hit.GetComponent<nodeStat>().getIndex();
                        if (CaveIsReachable(hitIndex))
                        {
                            root.GetComponent<johnRootController>().addVisitedIndex(hitIndex);
                            playerActualSpeed = playerSpeed;
                            if (caveIndex != hitIndex) isMoving = true;
                            caveIndex = hitIndex;
                            Debug.Log("Going to center of cave instead");
                            targetPosition = hit.transform.position;
                            Debug.Log("Player index: " + caveIndex.ToString());
                            //Send new index to camera
                            setExpectedTime();
                            camera.GetComponent<cameraController>().changePlayerIndex(caveIndex, expectedTime);
                            // Records cave move
                            numCaveMoves += 1;
                            SceneTransitionManager.Instance.currentGameStats.addVisitedNodeIndex(hitIndex);
                        }
                    }
                    //Check to see if debris is in the current cave
                    //Move to debris, set it to be destroyed, then move back
                    else if (hit.tag == "Debris")
                    {
                        if (!hit.GetComponent<debrisController>().getFlagDestroy())
                        {
                            cameraFollow = false;
                            //Check now if debris is in the same cave
                            if ((caveIndex == 1 && hit.GetComponent<debrisController>().getChildOfRoot()) || (!hit.GetComponent<debrisController>().getChildOfRoot() && hit.GetComponentInParent<nodeStat>().getIndex() == caveIndex))
                            {
                                playerActualSpeed = playerSpeed * 0.3f;
                                targetPosition = hit.transform.position;
                                setExpectedTime();
                                hit.GetComponent<debrisController>().setFlagDestroy();
                                Debug.Log("Moving to debris");
                                clearDebris = true;
                            }
                        }
                    }
                    //Check to see if a cave exit was clicked, then use its index to get the new cave
                    else if (hit.tag == "CaveExit")
                    {
                        playerActualSpeed = playerSpeed;
                        int tempIndex = hit.GetComponent<caveExitController>().targetIndex;
                        int exitCaveIndex = hit.GetComponent<caveExitController>().originIndex;
                        if (tempIndex != caveIndex && exitCaveIndex == caveIndex)
                        {
                            if (tempIndex == 1)
                            {
                                root.GetComponent<johnRootController>().addVisitedIndex(tempIndex);
                                caveIndex = 1;
                                targetPosition = root.transform.position;
                            }
                            else if (CaveIsReachable(tempIndex))
                            {
                                root.GetComponent<johnRootController>().addVisitedIndex(tempIndex);
                                caveIndex = tempIndex;
                                targetPosition = root.GetComponent<johnRootController>().findObject(caveIndex).transform.position;
                            }
                            setExpectedTime();
                            camera.GetComponent<cameraController>().changePlayerIndex(caveIndex, expectedTime);
                            Debug.Log("Going to cave this exits to.");
                            Debug.Log("Player index: " + caveIndex.ToString());
                            root.GetComponent<johnRootController>().addVisitedIndex(tempIndex);
                            caveIndex = tempIndex;
                            targetPosition = root.GetComponent<johnRootController>().findObject(caveIndex).transform.position;

                            // Records cave move
                            numCaveMoves += 1;
                            SceneTransitionManager.Instance.currentGameStats.addVisitedNodeIndex(tempIndex);
                        }
                    }

                    //Back to working with any target position
                    targetPosition.z = transform.position.z;

                    //Reset elapsed time. The expected travel time has already been set
                    elapsedTime = 0;

                    //Get the directional vector from the player's location to the mouse input
                    Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
                    //Rotate toward mouse input
                    transform.up = direction;
                }
            }
        }
    }

    //Set an expected travel time, speeding it up if travel would take longer than maxTime seconds
    void setExpectedTime()
    {
        Debug.Log((targetPosition - storePosition).magnitude);
        float maxTime = 2;
        expectedTime = (targetPosition - storePosition).magnitude;
        if (expectedTime / playerActualSpeed > maxTime)
        {
            expectedTime = maxTime;
        }
        else
        {
            expectedTime /= playerActualSpeed;
        }
    }

    //True if the cave with index targetIndex is reachable from the cave with index caveIndex
    private bool CaveIsReachable(int targetIndex)
    {
        //May want to add some check here for gamemode, since it's not needed for
        //something like breadth-first search
        bool optimalMove = root.GetComponent<johnRootController>().optimalMoveToParent(targetIndex, caveIndex);
        if (!optimalMove)
        {
            Debug.Log("Go back! Bad move!");
        }

        //Get the current node and find if it has debris blocking the path
        //Not necessary in checking for parent node access, any debris is already destroyed
        bool left = false;
        bool right = false;
        if (caveIndex == 1)
        {
            left = root.GetComponent<johnRootController>().getLeftDebris();
            right = root.GetComponent<johnRootController>().getRightDebris();
        }
        else
        {
            GameObject currentCave = root.GetComponent<johnRootController>().findObject(caveIndex);
            left = currentCave.GetComponent<nodeStat>().getLeftDebris();
            right = currentCave.GetComponent<nodeStat>().getRightDebris();
        }
        //Checks both for a valid target cave's index, and if debris blocks the path we want to take
        if (targetIndex == caveIndex * 2)
        {
            return !left;
        }
        else if (targetIndex == caveIndex * 2 + 1)
        {
            return !right;
        }
        else if (targetIndex == caveIndex / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // FixedUpdate is called at a fixed interval. Use for physics code.
    private void FixedUpdate()
    {
        //If the player performed an action that causes the player to move,
        //the values below will be set and elapsed time will be 0 initially.
        if (elapsedTime < expectedTime)
        {
            elapsedTime += Time.deltaTime;
            float timePercent = elapsedTime / expectedTime;
            transform.position = Vector3.Lerp(storePosition, targetPosition, timePercent);
            if (!isMoving)
            {
                isMoving = true;
            }
        }
        //If the player completed moving but was clearing debris, clear it and then
        //swap the stored and target positions while resetting elapsed time to 0.
        //The expected time needs no change because it's the same distance.
        else if (clearDebris)
        {
            //Doesn't work because of this function type, want to wait a second
            //yield return new WaitForSeconds(1.5f);
            targetPosition = storePosition;
            storePosition = transform.position;
            Debug.Log("Moving away from debris");
            clearDebris = false;
            elapsedTime = 0;
        }
        //Perform this when the player isn't doing something that will cause them to move
        else
        {
            //Allow processes that require the player to be still
            isMoving = false;

            //Rotate player toward mouse when not moving
            FaceMouse();
        }
    }

    // Face the player sprite toward the current mouse position
    void FaceMouse()
    {
        //Get the current mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get the directional vector from the player's location to the mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        //Rotate toward mouse position
        transform.up = direction;
    }

    //Trigger when the player enters the cave the miner is in
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.tag == "RandomMiner")
        {
            Debug.Log("Got to the miner!");
            foundMiner = true;
        }
    }

    //Reset the number of moves the player has made and their position at the root
    public void resetPlayer()
    {
        numCaveMoves = 0;
        numMinerShouts = 0;
        numDetonations = 0;
        transform.position = root.transform.position;
        targetPosition = transform.position;
        caveIndex = 1;
        camera.GetComponent<cameraController>().changePlayerIndex(caveIndex, 0);
    }

    //Called by the minerSearchGame script, increments the number of shouts made
    public void incMinerShout()
    {
        numMinerShouts += 1;
    }

    //Called by debrisController script, increments number of explosions and
    //number of detonations (times detonate is pressed with at least one explosion)
    public void incExplosions()
    {
        numExplosions += 1;
        detonation = true;
        Invoke("incDetonations", 0.5f);
    }

    //Increment number of detonations without counting multiple explosions at once
    void incDetonations()
    {
        if (detonation)
        {
            detonation = false;
            numDetonations += 1;
        }
    }

    public int getIndex()
    {
        return caveIndex;
    }

    public bool checkMoving()
    {
        return isMoving;
    }

    public bool checkMinerFound()
    {
        return foundMiner;
    }

    public int getNumCaveMoves()
    {
        return numCaveMoves;
    }

    public int getNumExplosions()
    {
        return numExplosions;
    }

    public int getNumDetonations()
    {
        return numDetonations;
    }

    public int getNumMinerShouts()
    {
        return numMinerShouts;
    }

    public bool getCameraFollow()
    {
        return cameraFollow;
    }
}
