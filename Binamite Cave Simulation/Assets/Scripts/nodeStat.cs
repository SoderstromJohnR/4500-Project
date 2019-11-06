using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeStat : MonoBehaviour
{
    public int depth;
    public int index;
    private float yPosition;
    private int currentDepth;
    // Start is called before the first frame update
    void Start()
    {
        currentDepth = 1;

        setDepth();
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
