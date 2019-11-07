using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class johnRootController : MonoBehaviour
{
    
    public GameObject node;
    public GameObject path;
    public int maxDepth;
    public float halfWidth;
    public float depthDistance;
    public float chanceCompleteTree;
    private int indexCount;
    private int currentDepth;
    private float startXDim;
    private GameObject theNode;
    private int tempIndex;

    private bool activate;
    private GameObject parentNode;
    private GameObject leftChild;
    private GameObject rightChild;
    public int totalNodes;



    void Start()
    {
        activate = true;
        totalNodes = 1;
        //makePaths();

        //Should get half of the x distance shown
        //startXDim = Camera.main.orthographicSize;
        //Now keep smaller for smaller trees, closer to full size as we add to the depth
        //startXDim = (maxDepth / (maxDepth + 1)) * startXDim;
        startXDim = halfWidth * ((float)maxDepth / (float)(maxDepth + 2));
        currentDepth = 1;
        tempIndex = 2;
        indexCount = 1;

        float typeCaveNetwork = Random.Range(0.0f, 1.0f);
        print(typeCaveNetwork);
        if (typeCaveNetwork < chanceCompleteTree)
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
                createChildCave(currentX - deltaX, deltaY, index);
                createIncompleteCaves(depth + 1, divX, currentX - deltaX, index * 2);
            }
            //Right child, add deltaX
            if (rightChild < .85)
            {
                createChildCave(currentX + deltaX, deltaY, index + 1);
                createIncompleteCaves(depth + 1, divX, currentX + deltaX, (index + 1) * 2);
            }
        }
    }

    void createChildCave(float newX, float newY, int index)
    {
        GameObject newNode;
        newNode = Instantiate(node, new Vector3(newX, transform.position.y - newY, 0), transform.rotation);
        newNode.GetComponent<nodeStat>().setIndex(index);
        totalNodes += 1;
    }


    void Update()
    {
        //setIndex function call here with switch because it needs all nodes to be created before running
        //this calls setIndex once for each depth (excluding zero)
        if (activate)
        {
            //Not needed with the current functions setting index in the process
            //setIndex(maxDepth);
            setPaths(totalNodes);
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
        //get size of sprite used
        float spriteSize = path.GetComponent<SpriteRenderer>().sprite.rect.width / path.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        scale.x = Vector3.Distance(_initialPosition, _finalPosition) / spriteSize;      
            _sprite.transform.localScale = scale;
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
        if (n > 1)
        {
            setIndex(n - 1);
        }
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
    public void setPaths(int n)
    {
        if (n > 2)
        {
            setPaths(n - 1);
        }
        int index = n;
        int parentIndex = 0;
        //put nodes in array
        GameObject[] nodes;
        nodes = GameObject.FindGameObjectsWithTag("Node");
        //set the child nodes indices
        int leftChildIndex = index * 2;
        int rightChildIndex = leftChildIndex + 1;
        bool leftTrue = false;
        bool rightTrue = false;
        //set the parent and child nodes
        foreach (GameObject currentNode in nodes)
        {
            if (currentNode.GetComponent<nodeStat>().index == index)
            {
                parentNode = currentNode;               
            }
            if (currentNode.GetComponent<nodeStat>().index == leftChildIndex)
            {
                leftChild = currentNode;
                leftTrue = true;
            }
            if (currentNode.GetComponent<nodeStat>().index == rightChildIndex)
            {
                rightChild = currentNode;
                rightTrue = true;
            }
        }
        //draw to the root
        if (index == 2 || index == 3)
        {
            Stretch(Instantiate(path), parentNode.transform.position, gameObject.transform.position, true);
        }
        //draw all other paths
        if (parentNode != null)
        {
            if (leftTrue)
            {
                Stretch(Instantiate(path), parentNode.transform.position, leftChild.transform.position, true);
            }
            if (rightTrue)
            {
                Stretch(Instantiate(path), parentNode.transform.position, rightChild.transform.position, true);
            }
        }
        
        


    }
}
