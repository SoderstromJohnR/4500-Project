﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class johnRootController : MonoBehaviour
{
    
    public GameObject node;
    public GameObject path;
    public int maxDepth;
    public float halfWidth;
    public float depthDistance;
    private int indexCount;
    private int currentDepth;
    private float startXDim;
    private GameObject theNode;
    private int tempIndex;

    private bool activate;
    



    void Start()
    {
        activate = true;
       
        //makePaths();

        //Should get half of the x distance shown
        //startXDim = Camera.main.orthographicSize;
        //Now keep smaller for smaller trees, closer to full size as we add to the depth
        //startXDim = (maxDepth / (maxDepth + 1)) * startXDim;
        startXDim = halfWidth * ((float)maxDepth / (float)(maxDepth + 2));
        currentDepth = 1;
        tempIndex = 2;
        indexCount = 1;
       

        createCaves();

        
    }

    void createCaves(int depth = 1 , int divX = 1, float currentX = 0.0f)
    {
        divX = divX * 2;
        float deltaX = startXDim / divX;
        float deltaY = depth * depthDistance;
        if (depth <= maxDepth)
        {
            

            //Left child, subtract deltaX
            createChildCave(currentX - deltaX, deltaY);
            createCaves(depth + 1, divX, currentX - deltaX);
            //Right child, add deltaX
            createChildCave(currentX + deltaX, deltaY);
            createCaves(depth + 1, divX, currentX + deltaX);

            

        }
        
    }

    void createChildCave(float newX, float newY)
    {
        GameObject newNode;
        newNode = Instantiate(node, new Vector3(newX, transform.position.y - newY, 0), transform.rotation);
        newNode.GetComponent<nodeStat>().setIndex(++indexCount);
    }

    void Update()
    {
        //setIndex function call here with switch because it needs all nodes to be created before running
        //this calls setIndex once for each depth (excluding zero)
        if (activate)
        {
            int count = 1;
            while(count <= maxDepth)
            {
                setIndex(count);
                count++;
            }
            
            activate = false;
        }
    }
    
    //stretch function
    public void Stretch(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1, 1, 1);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition);
        _sprite.transform.localScale = scale;
    }
    public void makePaths()
    {
        
    }
    public GameObject findObject(int n)
    {
        GameObject[] nodes;
        
        nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject currentNode in nodes)
        {
            if (currentNode.GetComponent<nodeStat>().index == n)
            {
                theNode = currentNode;
            }
        }
        return theNode;
    }
    public void setIndex(int n)
    {
        GameObject[] nodes;

        nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject currentNode in nodes)
        {

            //get nodes at depth n and set their index

            if (currentNode.GetComponent<nodeStat>().depth == n)
            {
                currentNode.GetComponent<nodeStat>().index = tempIndex++;
            }      
        }
        
    }
}
