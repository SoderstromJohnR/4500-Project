﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeStat : MonoBehaviour
{
    public int depth;
    public int index;
    private float yPosition;
    private int currentDepth;
    protected bool leftDebris;
    protected bool rightDebris;
    private GameObject debris;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "Cave";
        

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

    //Add debris based on passed boolean values for each side, no debris needed for parent
    public void setDebris(float xDim, float depthDistance, bool left, bool right)
    {
        debris = Resources.Load<GameObject>("basicDebrisPlaceholder");
        GameObject newDebris;
        //Calculate necessary values to place and angle debris correctly
        float deltaX = xDim;
        //Didn't see a good way to wait for the depth to be set, unfortunately
        for (int i = 1; i <= index; i *= 2)
        {
            deltaX /= 2;
        }
        float deltaY = depthDistance;
        Vector3 size = GetComponent<Renderer>().bounds.size;
        float distance = size.y / 2.5f;
        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        //Place the debris using above values if they are set to true
        if (left)
        {
            //deltaX is the value toward the right child, multiple by -1 to get left
            Vector3 insPosition = transform.position + Quaternion.AngleAxis(angle + 180, Vector3.forward) * transform.right * distance;
            newDebris = Instantiate(debris, insPosition, Quaternion.AngleAxis(angle - 90, Vector3.forward), transform);
            newDebris.GetComponent<debrisController>().setIsLeftDebris(true);
            newDebris.GetComponent<debrisController>().setChildOfRoot(false);
        }
        if (right)
        {
            //deltaX is the value toward the right child, multiple by -1 to get left
            Vector3 insPosition = transform.position + Quaternion.AngleAxis(-1 * angle, Vector3.forward) * transform.right * distance;
            newDebris = Instantiate(debris, insPosition, Quaternion.AngleAxis(90 - angle, Vector3.forward), transform);
            newDebris.GetComponent<debrisController>().setIsLeftDebris(false);
            newDebris.GetComponent<debrisController>().setChildOfRoot(false);
        }
    }

    public int getIndex()
    {
        return index;
    }

    public bool getLeftDebris()
    {
        return leftDebris;
    }

    public bool getRightDebris()
    {
        return rightDebris;
    }

    public void removeLeftDebris()
    {
        leftDebris = false;
    }

    public void removeRightDebris()
    {
        rightDebris = false;
    }
}
