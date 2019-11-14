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
<<<<<<< Updated upstream
=======
    public float chanceCompleteTree;
    public bool isLimitedToTotalNodes;
    public int limitedTreeNodes;
>>>>>>> Stashed changes
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
    private int maxIndex;
    private bool maxIndexSwitch;



    void Start()
    {
        activate = true;
        totalNodes = 1;
        maxIndex = 0;
        maxIndexSwitch = true;
        //makePaths();

        //Should get half of the x distance shown
        //startXDim = Camera.main.orthographicSize;
        //Now keep smaller for smaller trees, closer to full size as we add to the depth
        //startXDim = (maxDepth / (maxDepth + 1)) * startXDim;
        startXDim = halfWidth * ((float)maxDepth / (float)(maxDepth + 2));
        currentDepth = 1;
        tempIndex = 2;
        indexCount = 1;
       

<<<<<<< Updated upstream
        createCaves();
=======
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
>>>>>>> Stashed changes

        
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

<<<<<<< Updated upstream
    void createChildCave(float newX, float newY)
=======
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
>>>>>>> Stashed changes
    {
        GameObject newNode;
        newNode = Instantiate(node, new Vector3(newX, transform.position.y - newY, 0), transform.rotation);
        totalNodes += 1;
        //newNode.GetComponent<nodeStat>().setIndex(++indexCount);
    }

    void Update()
    {
        //setIndex function call here with switch because it needs all nodes to be created before running
        //this calls setIndex once for each depth (excluding zero)
        if (activate)
        {
<<<<<<< Updated upstream
            setIndex(maxDepth);
            setPaths(totalNodes);
=======
            //Not needed with the current functions setting index in the process
            //setIndex(maxDepth);
            setMaxIndex();
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======

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

>>>>>>> Stashed changes
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
        //int parentIndex = 0;
        //put nodes in array
        GameObject[] nodes;
        nodes = GameObject.FindGameObjectsWithTag("Node");
        //set the child nodes indices
        int leftChildIndex = index * 2;
        int rightChildIndex = leftChildIndex + 1;
        bool leftTrue = false;
        bool rightTrue = false;
        bool parentTrue = false;
        //set the parent and child nodes
        foreach (GameObject currentNode in nodes)
        {
            if (currentNode.GetComponent<nodeStat>().index == index)
            {
                parentNode = currentNode;
                parentTrue = true;
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
        //turn off maxIndex check
        maxIndexSwitch = false;
        //draw to the root
        if (parentTrue)
        {
            if (index == 2 || index == 3)
            {
                GameObject basket = Instantiate(path);
                Stretch(basket, parentNode.transform.position, gameObject.transform.position, true);
                pathStat script = basket.GetComponent<pathStat>();
                //set path index
                script.index = index;
            }
        }
        
        //draw all other paths
        if (parentNode != null)
        {
            if (leftTrue)
            { 
                GameObject basket = Instantiate(path);
                Stretch(basket, parentNode.transform.position, leftChild.transform.position, true);
                pathStat script = basket.GetComponent<pathStat>();
                //set path index
                script.index = leftChildIndex;
            }
            if (rightTrue)
            {
                GameObject basket = Instantiate(path);
                Stretch(basket, parentNode.transform.position, rightChild.transform.position, true);
                pathStat script = basket.GetComponent<pathStat>();
                //set path index
                script.index = rightChildIndex;
            }
        }
        
        


    }
<<<<<<< Updated upstream
=======
    //function to set maxIndex --> also calls setPaths
    void setMaxIndex()
    {
        GameObject[] nodes;
        nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject currentNode in nodes)
        {
            if (currentNode.GetComponent<nodeStat>().index > maxIndex)
            {
                maxIndex = currentNode.GetComponent<nodeStat>().index;
            }
        }
        setPaths(maxIndex);
    }

    //Determines a random cave, currently at the max depth, to create a lost miner
    void setRandomMiner()
    {
        GameObject minerNode = getRandomNodeMaxDepth();
        Instantiate(miner, minerNode.transform);
    }
>>>>>>> Stashed changes
}
