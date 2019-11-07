using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rootController : MonoBehaviour
{
    
    public GameObject node;
    public GameObject path;
    public int maxDepth;
    private int indexCount;
    private GameObject theNode;



    void Start()
    {
        //makePaths();
        
        indexCount = 1;
        GameObject newNode;
        //create nodes at depth 1
        if (maxDepth == 1)
        {   
            //depth 1
                //instantiate left child
                newNode = Instantiate(node, new Vector3(transform.position.x - .75f, transform.position.y - 2, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
            //instantiate right child
                newNode = Instantiate(node, new Vector3(transform.position.x + .75f, transform.position.y - 2, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
        }
        if (maxDepth == 2)
        {
            //depth 1
                //instantiate left child of root
                newNode = Instantiate(node, new Vector3(transform.position.x - 1.25f, transform.position.y - 2, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
                //instantiate right child of root
                newNode = Instantiate(node, new Vector3(transform.position.x + 1.25f, transform.position.y - 2, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
            //depth 2
                //instantiate left subtree
                newNode = Instantiate(node, new Vector3(transform.position.x - 2f, transform.position.y - 4, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
                newNode = Instantiate(node, new Vector3(transform.position.x - .5f , transform.position.y - 4, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
                //instantiate right subtree               
                newNode = Instantiate(node, new Vector3(transform.position.x + .5f , transform.position.y - 4, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
                newNode = Instantiate(node, new Vector3(transform.position.x + 2f, transform.position.y - 4, 0), transform.rotation);
                newNode.GetComponent<nodeStat>().setIndex(++indexCount);
        }
        if (maxDepth == 3)
        {
            //depth 1
                //instantiate left child
                Instantiate(node, new Vector3(transform.position.x - 1.9f, transform.position.y - 2, 0), transform.rotation);
                //instantiate right child
                Instantiate(node, new Vector3(transform.position.x + 1.9f, transform.position.y - 2, 0), transform.rotation);
            //depth 2
                //instantiate left children
                Instantiate(node, new Vector3(transform.position.x - 2.85f, transform.position.y - 4, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - .95f, transform.position.y - 4, 0), transform.rotation);
                //instantiate right subtree
                Instantiate(node, new Vector3(transform.position.x + 2.85f, transform.position.y - 4, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + .95f, transform.position.y - 4, 0), transform.rotation);
            //depth 3
                //instantiate left subtree
                Instantiate(node, new Vector3(transform.position.x - 3.4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - 2.3f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - 1.5f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - .4f, transform.position.y - 6, 0), transform.rotation);
                //instantiate right subtree
                Instantiate(node, new Vector3(transform.position.x + 3.4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + 2.3f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + 1.5f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + .4f, transform.position.y - 6, 0), transform.rotation);
        }
        if (maxDepth == 4)
        {
            //depth 1
            //instantiate left child
                Instantiate(node, new Vector3(transform.position.x - 3.2f, transform.position.y - 2, 0), transform.rotation);
                //instantiate right child
                Instantiate(node, new Vector3(transform.position.x + 3.2f, transform.position.y - 2, 0), transform.rotation);
            //depth 2
                //instantiate left children
                Instantiate(node, new Vector3(transform.position.x - 4.8f, transform.position.y - 4, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - 1.6f, transform.position.y - 4, 0), transform.rotation);
                //instantiate right subtree
                Instantiate(node, new Vector3(transform.position.x + 4.8f, transform.position.y - 4, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + 1.6f, transform.position.y - 4, 0), transform.rotation);
            //depth 3
                //instantiate left subtree
                Instantiate(node, new Vector3(transform.position.x - 5.6f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - 4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - 2.4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x - .8f, transform.position.y - 6, 0), transform.rotation);
                //instantiate right subtree
                Instantiate(node, new Vector3(transform.position.x + 5.6f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + 4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + 2.4f, transform.position.y - 6, 0), transform.rotation);
                Instantiate(node, new Vector3(transform.position.x + .8f, transform.position.y - 6, 0), transform.rotation);
            //depth 4
            //left
            int count = 0;
            float place = .4f;
            while (count < 8)
            {
                Instantiate(node, new Vector3(transform.position.x + place, transform.position.y - 8, 0), transform.rotation);
                count += 1;
                place += .8f;
            }
            //right
            count = 0;
            place = .4f;
            while (count < 8)
            {
                Instantiate(node, new Vector3(transform.position.x - place, transform.position.y - 8, 0), transform.rotation);
                count += 1;
                place += .8f;
            }
                
        }
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
