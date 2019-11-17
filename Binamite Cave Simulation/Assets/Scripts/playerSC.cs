using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSC : MonoBehaviour
{
    public float playerSpeed = 10;
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

    // Start is called before the first frame update
    void Start()
    {
        //Set the target position immediately to the player's starting location
        targetPosition = transform.position;
        caveIndex = 1;
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
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit2D hit = Physics2D.Raycast(click2D, Vector2.zero);
            if (hit.transform != null && hit.collider.gameObject.name == "Root Node")
            {
                if (CaveIsReachable(1))
                {
                    playerActualSpeed = playerSpeed;
                    if (caveIndex != 1) isMoving = true;
                    caveIndex = 1;
                    Debug.Log("Going to center of entrance instead");
                    targetPosition = hit.transform.gameObject.transform.position;
                }
            }
            else if (hit.transform != null && hit.collider.gameObject.name == "Node(Clone)")
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
                }

            }
            //Check to see if debris is in the current cave
            //Move to debris, set it to be destroyed, then move back
            else if (hit.transform != null && hit.collider.gameObject.tag == "Debris")
            {
                GameObject debris = hit.collider.gameObject;
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

            //Send new index to camera
            camera.GetComponent<cameraController>().changePlayerIndex(caveIndex);

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
        return targetIndex == caveIndex * 2  && !left
            || targetIndex == caveIndex * 2 + 1 && !right
            || targetIndex == caveIndex / 2;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.tag == "RandomMiner")
        {
            Debug.Log("Got to the miner!");
            foundMiner = true;
        }
    }
}
