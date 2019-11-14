using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class johnRootController : MonoBehaviour
{
    
    public GameObject node;
    public GameObject path;
    public GameObject miner;
    public int maxDepth;
    public float halfWidth;
    public float depthDistance;
    public float chanceCompleteTree;
    public bool isLimitedToTotalNodes;
    public int limitedTreeNodes;
    private int indexCount;
    private int currentDepth;
    private float startXDim;
    private GameObject theNode;
    private int tempIndex;
    private List<int> nodeIndices = new List<int>(); //Added to track all indices of created nodes

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
        nodeIndices.Add(1); //Index 1 will not be added through functions, it already exists

        if (isLimitedToTotalNodes)
        {
            createLimitedNumberCaves();
        }
        else
        {
            float typeCaveNetwork = Random.Range(0.0f, 1.0f);
            if (typeCaveNetwork < chanceCompleteTree)
            {
                createCompleteCaves();
            }
            else
            {
                createIncompleteCaves();
            }
        }

        setRandomMiner();
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
    void createIncompleteCaves(int depth = 1, int divX = 1, float currentX = 0.0f, int index = 2, float chanceOfChildren = 1.0f)
    {
        divX = divX * 2;
        float deltaX = startXDim / divX;
        float deltaY = depth * depthDistance;
        float leftChild = Random.Range(0.0f, 1.0f);
        float rightChild = Random.Range(0.0f, 1.0f);
        if (depth <= maxDepth)
        {
            //Left child, subtract deltaX
            if (leftChild < chanceOfChildren)
            {
                createChildCave(currentX - deltaX, deltaY, index);
                createIncompleteCaves(depth + 1, divX, currentX - deltaX, index * 2, chanceOfChildren - .05f);
            }
            //Right child, add deltaX
            if (rightChild < chanceOfChildren)
            {
                createChildCave(currentX + deltaX, deltaY, index + 1);
                createIncompleteCaves(depth + 1, divX, currentX + deltaX, (index + 1) * 2, chanceOfChildren - .05f);
            }
        }
    }

    //Create a random cave newtowrk based on the total number of nodes
    //Sends message to debug log if it exceeds the possible total number based on max depth
    void createLimitedNumberCaves()
    {
        //Determine max number of caves, output message to debug log if it's higher than expected
        //Cut totalNodes if it is too high
        int expectedMaxCaves = 1;
        for (int i = 0; i <= maxDepth; i++)
        {
            expectedMaxCaves *= 2;
        }
        expectedMaxCaves--;
        if (expectedMaxCaves < limitedTreeNodes)
        {
            Debug.Log("Check number of total nodes and max depth. totalNodes is higher than the possible value at maxDepth.");
            limitedTreeNodes = expectedMaxCaves;
        }
        
        //If totalNodes is greater than expected, we want it clamped here. We run this after checking totalNodes.
        int[] caveIndices = new int[limitedTreeNodes];
        caveIndices[0] = 1;
        int numIndicesInArray = 1;
        
        //Initialize all array elements after the first to 0
        for (int i = 1; i < limitedTreeNodes; i++)
        {
            caveIndices[i] = 0;
        }
        
        //Create indices to max depth before randomly generating others if there are enough
        //Increment tally of indices in array with each index
        for (int i = 0; i < maxDepth; i++)
        {
            if (numIndicesInArray >= limitedTreeNodes)
            {
                continue;
            }
            if (Random.Range(0.0f, 1.0f) < .5)
            {
                caveIndices[i + 1] = caveIndices[i] * 2;
            }
            else
            {
                caveIndices[i + 1] = caveIndices[i] * 2 + 1;
            }
            numIndicesInArray++;
        }
        
        //Determine all indices to create in this cave network
        while (numIndicesInArray < limitedTreeNodes)
        {
            bool isInArray = false;
            int newCaveIndex = 0;

            //Choose a random cave and select a child index to add
            int currentIndex = Random.Range(0, numIndicesInArray);
            if (Random.Range(0.0f, 1.0f) < .5)
            {
                newCaveIndex = caveIndices[currentIndex] * 2;
            }
            else
            {
                newCaveIndex = caveIndices[currentIndex] * 2 + 1;
            }

            //Start over if the new cave goes below the max depth
            if (newCaveIndex <= expectedMaxCaves)
            {
                //Check if the new cave already exists
                for (int i = 0; i < numIndicesInArray; i++)
                {
                    if (caveIndices[i] == newCaveIndex)
                    {
                        isInArray = true;
                    }
                }

                //Add new cave index to the array and increment
                if (!isInArray)
                {
                    caveIndices[numIndicesInArray] = newCaveIndex;
                    numIndicesInArray++;
                }
            }
        }

        //Call recursive cave creation
        createLimitedCaveNetwork(caveIndices);
    }

    //Recursive Cave Network Creation specifically for createLimitedNumberCaves()
    void createLimitedCaveNetwork(int[] caveArray, int depth = 1, int divX = 1, float currentX = 0.0f, int index = 1)
    {
        bool leftInArray = false;
        bool rightInArray = false;
        divX = divX * 2;
        float deltaX = startXDim / divX;
        float deltaY = depth * depthDistance;

        //Check if either child is in our array of nodes, remember first value is 1
        for (int i = 1; i < limitedTreeNodes; i++)
        {
            if (caveArray[i] == index * 2)
            {
                leftInArray = true;
            }
            if (caveArray[i] == (index * 2) + 1)
            {
                rightInArray = true;
            }
        }

        //Create left and/or child if they exist and move into them
        if (leftInArray)
        {
            createChildCave(currentX - deltaX, deltaY, index * 2);
            createLimitedCaveNetwork(caveArray, depth + 1, divX, currentX - deltaX, index * 2);
        }
        if (rightInArray)
        {
            createChildCave(currentX + deltaX, deltaY, index * 2 + 1);
            createLimitedCaveNetwork(caveArray, depth + 1, divX, currentX + deltaX, index * 2 + 1);
        }
    }

    void createChildCave(float newX, float newY, int index)
    {
        GameObject newNode;
        newNode = Instantiate(node, new Vector3(newX, transform.position.y - newY, 0), transform.rotation);
        newNode.GetComponent<nodeStat>().setIndex(index);
        totalNodes += 1;
        nodeIndices.Add(index);
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

    //Makes use of tree's max depth and the findObject function above to select a random node that exists in the max depth
    //
    public GameObject getRandomNodeMaxDepth()
    {
        //Create a list for indices of nodes that actually exist
        List<int> possibleIndices = new List<int>();

        //Determine the index values of the max depth
        int lowerBound = 1;
        for (int i = 0; i < maxDepth; i++)
        {
            lowerBound *= 2;
        }

        //Extract all existing nodes at the max depth
        //If there are none, reduce the depth by 1
        while (possibleIndices.Count == 0)
        {
            for (int i = lowerBound; i < lowerBound * 2; i++)
            {
                if (nodeIndices.Contains(i))
                {
                    possibleIndices.Add(i);
                }
            }
            if (possibleIndices.Count == 0)
            {
                lowerBound /= 2;
            }
        }

        //Choose a random element from the list we just created and return that node
        int randomIndex = Random.Range(0, possibleIndices.Count);
        return findObject(possibleIndices[randomIndex]);
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

    //Determines a random cave, currently at the max depth, to create a lost miner
    void setRandomMiner()
    {
        GameObject minerNode = getRandomNodeMaxDepth();
        Instantiate(miner, minerNode.transform);
    }

    //Give read-only access to list of cave indices
    public List<int> getNodeIndices()
    {
        return nodeIndices;
    }
}
