using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSC : MonoBehaviour
{
    public float playerSpeed = 10;

    private int caveIndex;
    private float playerActualSpeed;
    private float targetDistance;
    private bool isMoving = false;
    private bool foundMiner = false;
    private Vector3 targetPosition;

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
        if (Input.GetMouseButtonDown(0))
        {
            //Get world coordinates of mouse input
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 target2D = new Vector2(targetPosition.x, targetPosition.y);

            //Check for click on object, go to center of object instead of mouse click
            Ray ray = Camera.main.ScreenPointToRay(targetPosition);
            RaycastHit2D hit = Physics2D.Raycast(target2D, Vector2.zero);
            if (hit.transform != null && hit.collider.gameObject.name == "Root Node")
            {
                caveIndex = 1;
                Debug.Log("Going to center of entrance instead");
                targetPosition = hit.transform.gameObject.transform.position;
            }
            else if (hit.transform != null && hit.collider.gameObject.name == "Node(Clone)")
            {
                caveIndex = hit.collider.gameObject.GetComponent<nodeStat>().getIndex();
                Debug.Log("Going to center of cave instead");
                targetPosition = hit.transform.gameObject.transform.position;
                Debug.Log("Player index: " + caveIndex.ToString());
            }

            //Back to working with any target position
            targetPosition.z = transform.position.z;

            //Get the directional vector from the player's location to the mouse input
            Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            //Rotate toward mouse input
            transform.up = direction;
        }
    }

    // FixedUpdate is called at a fixed interval. Use for physics code.
    private void FixedUpdate()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, playerSpeed * Time.deltaTime);

            //Stop processes that require the player to be still
            if (!isMoving)
            {
                isMoving = true;
            }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.tag == "RandomMiner")
        {
            Debug.Log("Got to the miner!");
            foundMiner = true;
        }
    }
}
