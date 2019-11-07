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



    void Start()
    {
        //makePaths();

        //Should get half of the x distance shown
        //startXDim = Camera.main.orthographicSize;
        //Now keep smaller for smaller trees, closer to full size as we add to the depth
        //startXDim = (maxDepth / (maxDepth + 1)) * startXDim;
        startXDim = halfWidth * ((float)maxDepth / (float)(maxDepth + 2));

        indexCount = 1;
        float typeCaveNetwork = Random.Range(0.0f, 1.0f);
        print (typeCaveNetwork);
        if (typeCaveNetwork < .01)
        {
            createCompleteCaves();
        }
        else
        {
            createIncompleteCaves();
        }
        
    }

    void createCompleteCaves(int depth = 1, int divX = 1, float currentX = 0.0f, int index = 2)
    {
        divX = divX * 2;
        float deltaX = startXDim / divX;
        float deltaY = depth * depthDistance;
        if (depth <= maxDepth)
        {
            //Left child, subtract deltaX
            createChildCave(currentX - deltaX, deltaY, index);
            createCompleteCaves(depth + 1, divX, currentX - deltaX, index * 2);
            //Right child, add deltaX
            createChildCave(currentX + deltaX, deltaY, index + 1);
            createCompleteCaves(depth + 1, divX, currentX + deltaX, (index + 1) * 2);
            
        } 
    }

    void createIncompleteCaves(int depth = 1, int divX = 1, float currentX = 0.0f, int index = 2)
    {
        divX = divX * 2;
        float deltaX = startXDim / divX;
        float deltaY = depth * depthDistance;
        float leftChild = Random.Range(0.0f, 1.0f);
        float rightChild = Random.Range(0.0f, 1.0f);
        if (depth <= maxDepth)
        {
            //Left child, subtract deltaX
            if (leftChild < .85)
            {
                createChildCave(currentX - deltaX, deltaY, index );
                createIncompleteCaves(depth + 1, divX, currentX - deltaX, index * 2);
            }
            //Right child, add deltaX
            if (rightChild < .85)
            {
                createChildCave(currentX + deltaX, deltaY, index + 1);
                createIncompleteCaves(depth + 1, divX, currentX + deltaX, (index + 1) * 2 );
            }
        }
    }

    void createChildCave(float newX, float newY, int index)
    {
        GameObject newNode;
        newNode = Instantiate(node, new Vector3(newX, transform.position.y - newY, 0), transform.rotation);
        newNode.GetComponent<nodeStat>().setIndex(index);
    }

    void Update()
    {
        
    }
    private void Graveyard()
    {
        //alternate design for depth 2
        //depth 1
        //instantiate left child
        Instantiate(node, new Vector3(transform.position.x - .9f, transform.position.y - 1, 0), transform.rotation);
        //instantiate right child
        Instantiate(node, new Vector3(transform.position.x + .9f, transform.position.y - 1, 0), transform.rotation);
        //depth 2
        //instantiate left children
        Instantiate(node, new Vector3(transform.position.x - 1.35f, transform.position.y - 2, 0), transform.rotation);
        Instantiate(node, new Vector3(transform.position.x - .45f, transform.position.y - 2, 0), transform.rotation);
        //instantiate right children
        Instantiate(node, new Vector3(transform.position.x + 1.35f, transform.position.y - 2, 0), transform.rotation);
        Instantiate(node, new Vector3(transform.position.x + .45f, transform.position.y - 2, 0), transform.rotation);
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
}