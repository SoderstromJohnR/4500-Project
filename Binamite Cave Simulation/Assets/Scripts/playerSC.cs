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

    private bool isMoving = false;
    private bool foundMiner = false;
    private bool clearDebris = false;

    private Vector3 targetPosition;
    private Vector3 clickPosition;
    private Vector3 storePosition;

    private bool detonation;
    [SerializeField] private int numCaveMoves;
    [SerializeField] private int numExplosions;
    [SerializeField] private int numDetonations;
    [SerializeField] private int numMinerShouts;

    // Start is called before the first frame update
    void Start()
    {
        //Set the target position immediately to the player's starting location
        targetPosition = transform.position;
        caveIndex = 1;
        numCaveMoves = 0;
        numMinerShouts = 0;
        numDetonations = 0;
        detonation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clearDebris && !isMoving && Input.GetMouseButtonDown(0))
        {
            //Get world coordinates of mouse input
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 click2D = new Vector2(clickPosition.x, clickPosition.y);

            //Check for click on object, go to center of object instead of mouse click
            //LayerMask clickLayer = LayerMask.GetMask("Node", "Debris");
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit2D[] hitAll = Physics2D.RaycastAll(click2D, Vector2.zero);

            //Not my favorite solution, but it solves the issue of clicking on debris and it only registering the node
            RaycastHit2D hit = hitAll[0];
            foreach (RaycastHit2D temp in hitAll)
            {
                if (temp.collider.gameObject.tag == "Debris")
                {
                    hit = temp;
                }
            }

            if (hit.collider != null && hit.collider.gameObject.name == "Root Node")
            {
                if (CaveIsReachable(1))
                {
                    playerActualSpeed = playerSpeed;
                    if (caveIndex != 1) isMoving = true;
                    caveIndex = 1;
                    Debug.Log("Going to center of entrance instead");
                    targetPosition = hit.transform.gameObject.transform.position;
                    //Send new index to camera
                    camera.GetComponent<cameraController>().changePlayerIndex(caveIndex);
                    numCaveMoves += 1;
                }
            }
            else if (hit.collider != null && hit.collider.gameObject.name == "Node(Clone)")
            {
                int hitIndex = hit.collider.gameObject.GetComponent<nodeStat>().getIndex();
                if (CaveIsReachable(hitIndex))
                {
                    playerActualSpeed = playerSpeed;
                    if (caveIndex != hitIndex) isMoving = true;
                    caveIndex = hitIndex;
                    Debug.Log("Going to center of cave instead");
                    targetPosition = hit.transform.gameObject.transform.position;
                    Debug.Log("Player index: " + caveIndex.ToString());
                    //Send new index to camera
                    camera.GetComponent<cameraController>().changePlayerIndex(caveIndex);
                    numCaveMoves += 1;
                }
            }
            //Check to see if debris is in the current cave
            //Move to debris, set it to be destroyed, then move back
            else if (hit.collider != null && hit.collider.gameObject.tag == "Debris")
            {
                GameObject debris = hit.collider.gameObject;
                if (!debris.GetComponent<debrisController>().getFlagDestroy())
                {
                    //Check now if debris is in the same cave
                    if ((caveIndex == 1 && debris.GetComponent<debrisController>().getChildOfRoot()) || (!debris.GetComponent<debrisController>().getChildOfRoot() && debris.GetComponentInParent<nodeStat>().getIndex() == caveIndex))
                    {
                        playerActualSpeed = 3.0f;
                        storePosition = transform.position;
                        targetPosition = debris.transform.position;
                        debris.GetComponent<debrisController>().setFlagDestroy();
                        Debug.Log("Moving to debris");
                        clearDebris = true;
                    }
                }
            }

            //Back to working with any target position
            targetPosition.z = transform.position.z;

            //Get the directional vector from the player's location to the mouse input
            Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            //Rotate toward mouse input
            transform.up = direction;
        }
    }

    //True if the cave with index targetIndex is reachable from the cave with index caveIndex
    private bool CaveIsReachable(int targetIndex)
    {
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
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, playerActualSpeed * Time.deltaTime);

            //Stop processes that require the player to be still
            if (!isMoving)
            {
                isMoving = true;
            }
        }
        else if (clearDebris)
        {
            //Doesn't work because of this function type, want to wait a second
            //yield return new WaitForSeconds(1.5f);
            targetPosition = storePosition;
            Debug.Log("Moving away from debris");
            clearDebris = false;
        }
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
        camera.GetComponent<cameraController>().changePlayerIndex(caveIndex);
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
}
