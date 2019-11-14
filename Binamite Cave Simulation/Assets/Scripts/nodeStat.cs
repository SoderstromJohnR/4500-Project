using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeStat : MonoBehaviour
{
    public int depth;
    public int index;
    private float yPosition;
    private int currentDepth;
    public bool visited;
    public bool visiting;
    // Start is called before the first frame update
    void Start()
    {
        visited = false;
        GetComponent<SpriteRenderer>().sortingLayerName = "Cave";

        currentDepth = 1;

        setDepth();
        
    }

    // Update is called once per frame
    void Update()
    {
        //set the node to black if it it hasn't been visited
        if (!visited)
        {
            //Fetch the SpriteRenderer from the GameObject
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to black
            nodeImage.color = Color.black;
        }
    }
    public void setIndex(int n)
    {
        index = n;
    }
    public void setDepth()
    {
        //this function finds finds and sets the depth of the node
        //based on its current y position

        //find the controller for root node
        GameObject root = GameObject.Find("Root Node");
        johnRootController rc = root.GetComponent<johnRootController>();
        //set yPosition to Depth Distance (from johnRootController) minus Root Node's y position      
        yPosition = (root.transform.position.y - rc.depthDistance);
        //while nodes left set depth
        while (currentDepth <= rc.maxDepth)
        {
            if (transform.position.y == yPosition)
            {
                depth = currentDepth;
            }
            currentDepth += 1;
            yPosition -= rc.depthDistance;

        }
    }

    public int getIndex()
    {
        return index;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            visited = true;
            //Fetch the SpriteRenderer from the GameObject
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to green
            nodeImage.color = Color.green;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            visiting = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Fetch the SpriteRenderer from the GameObject
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to blue
            nodeImage.color = Color.blue;
        }
        visiting = false;
    }
}
