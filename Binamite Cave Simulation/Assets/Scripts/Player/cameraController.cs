using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject root;
    public float cameraMargin;

    private float currentSize;
    private Vector3 currentPosition;
    private float targetSize;
    private Vector3 targetPosition;
    private float fullSizeCamera;
    private Vector3 fullCaveCamera;
    private float zoomSizeCamera;
    private Vector3 zoomCaveCamera;
    private float nodeZoomSize;
    
    private float elapsedChangeZoom;
    private float elapsedTimeChange;
    private float expectedTimeChange;
    private bool nodesMade = false;
    private bool cameraFullScreen = true;
    private bool changingZoom = false;
    private bool nodeZoomCamera = false;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // LateUpdate runs once every frame after all items have been processed
    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        if (!nodesMade)
        {
            getCaveFullBounds();
            setFullCamera();
            getCaveZoomBounds(1);
            getNodeZoomSize();
            nodesMade = true;
        }

        //Change camera mode on input
        if (Input.GetKeyDown(KeyCode.M) && !changingZoom && !player.GetComponent<playerSC>().checkMoving())
        {
            changingZoom = true;
            cameraFullScreen = !cameraFullScreen;
            setCurrentAndTargets();
        }

        //Change zoomed in camera mode on input
        if (Input.GetKeyDown(KeyCode.N) && !cameraFullScreen && !changingZoom && !player.GetComponent<playerSC>().checkMoving())
        {
            changingZoom = true;
            nodeZoomCamera = !nodeZoomCamera;
            setCurrentAndTargets();
        }
        
        //Change the size of the camera based on where we're zooming
        //Smooth changes from full cave network to zoomed in
        if (changingZoom)
        {
            elapsedChangeZoom += Time.deltaTime;
            cam.orthographicSize = Mathf.SmoothStep(currentSize, targetSize, elapsedChangeZoom);
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedChangeZoom);
            if (elapsedChangeZoom > 1.0f)
            {
                changingZoom = false;
                elapsedChangeZoom = 0;
            }
        }

        //This was smoother than the previous method, zooms the camera so that the parent and children nodes, if any existed,
        //will always be visible in the partially zoomed in mode
        if (!cameraFullScreen && elapsedTimeChange <= expectedTimeChange)
        {
            elapsedTimeChange += Time.deltaTime;
            float elapsedTimePercent = elapsedTimeChange / expectedTimeChange;
            cam.orthographicSize = Mathf.SmoothStep(currentSize, targetSize, elapsedTimePercent);
            if (elapsedTimeChange > expectedTimeChange)
            {
                expectedTimeChange = 0;
            }
        }
        //Focus the camera on the player, placing it in the above if statement left the camera in an odd position
        if (!cameraFullScreen && player.GetComponent<playerSC>().checkMoving() && player.GetComponent<playerSC>().getCameraFollow())
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    //Set current size and position, and target size and position based on camera booleans from input
    //These will be used in the latter part of LateUpdate to move and zoom the camera
    void setCurrentAndTargets()
    {
        currentSize = cam.orthographicSize;
        currentPosition = transform.position;
        if (cameraFullScreen)
        {
            targetSize = fullSizeCamera;
            targetPosition = fullCaveCamera;
        }
        else if (!nodeZoomCamera)
        {
            targetSize = zoomSizeCamera;
            targetPosition = zoomCaveCamera;
        }
        else
        {
            targetSize = nodeZoomSize;
            targetPosition = zoomCaveCamera;
        }
    }

    //Get the maximum and minimum x and y coordinates for the full cave network camera
    //Then store into a class variable
    void getCaveFullBounds()
    {
        Vector3 nodeSize = root.GetComponent<Renderer>().bounds.size;
        List<int> nodeIndices = root.GetComponent<johnRootController>().getNodeIndices();
        int leftIndex = 1;
        int rightIndex = 1;

        float depthDistance = root.GetComponent<johnRootController>().depthDistance;
        int actualMaxDepth = 0;
        int maxNode = 0;

        //Find largest index to get the true real depth, in case it doesn't fill to max
        foreach (int i in nodeIndices)
        {
            if (maxNode < i)
            {
                maxNode = i;
            }
        }
        
        //Find actual max depth based on largest index, think of i as an index on the far left side
        for (int i = 2; i <= maxNode; i *= 2)
        {
            actualMaxDepth++;
        }
        
        //Determine left and right most nodes
        for (int i = 0; i < actualMaxDepth; i++)
        {
            if (nodeIndices.Contains(leftIndex * 2))
            {
                leftIndex = leftIndex * 2;
            }
            if (nodeIndices.Contains(rightIndex * 2 + 1))
            {
                rightIndex = rightIndex * 2 + 1;
            }
        }
        
        //Get x coordinates of right and leftmost nodes, and height
        float leftX = root.GetComponent<johnRootController>().findObject(leftIndex).transform.position.x;
        float rightX = root.GetComponent<johnRootController>().findObject(rightIndex).transform.position.x;
        float topY = root.transform.position.y;
        float yMin = topY - actualMaxDepth * depthDistance;
        
        //Calculate actual min and max x and y values
        float fullMinX = leftX - cameraMargin - (nodeSize.x / 2);
        float fullMaxX = rightX + cameraMargin + (nodeSize.x / 2);
        float fullMinY = yMin - cameraMargin - (nodeSize.y / 2);
        float fullMaxY = topY + cameraMargin + (nodeSize.y / 2);

        //Store center of cave system and size to show entire cave network
        fullCaveCamera = new Vector3((fullMaxX + fullMinX) / 2, (fullMaxY + fullMinY) / 2, transform.position.z);
        fullSizeCamera = Mathf.Max(((fullMaxX - fullMinX) / 2.0f) / cam.aspect, (fullMaxY - fullMinY) / 2.0f);
    }

    //Change camera position and size based on previously obtained 
    //minimum and maximum x and y values for full cave network
    void setFullCamera()
    {
        //Move center of camera to center and change size based on stored values
        transform.position = fullCaveCamera;
        cam.orthographicSize = fullSizeCamera;
    }

    //Get and store the coordinates for a camera zoomed on the player's current cave
    void getCaveZoomBounds(int index)
    {
        //Declare necessary variables
        GameObject caveNode;
        Vector3 nodePosition;
        Vector3 nodeSize;
        float baseXDistance = root.GetComponent<johnRootController>().getBaseXDistance();
        float depthDistance = root.GetComponent<johnRootController>().depthDistance;
        float zoomMaxX;
        float zoomMinX;
        float zoomMaxY;
        float zoomMinY;

        //Obtain position and size based either on root or child node
        if (index == 1)
        {
            nodePosition = root.transform.position;
            nodePosition.z = transform.position.z;
            nodeSize = root.GetComponent<Renderer>().bounds.size;
            zoomMaxX = baseXDistance / 2.0f + cameraMargin + (nodeSize.x / 2);
            zoomMinX = -1 * zoomMaxX;
            zoomMaxY = root.transform.position.y + depthDistance + cameraMargin + (nodeSize.y / 2);
            zoomMinY = root.transform.position.y - depthDistance - cameraMargin - (nodeSize.y / 2);
        }
        else
        {
            caveNode = root.GetComponent<johnRootController>().findObject(index);
            nodePosition = caveNode.transform.position;
            nodePosition.z = transform.position.z;
            nodeSize = caveNode.GetComponent<Renderer>().bounds.size;
            int depth = caveNode.GetComponent<nodeStat>().depth;
            baseXDistance = baseXDistance / Mathf.Pow(2.0f, depth);
            zoomMaxX = nodePosition.x + baseXDistance + cameraMargin + (nodeSize.x / 2);
            zoomMinX = nodePosition.x - baseXDistance - cameraMargin - (nodeSize.x / 2);
            zoomMaxY = nodePosition.y + depthDistance + cameraMargin + (nodeSize.y / 2);
            zoomMinY = nodePosition.y - depthDistance - cameraMargin - (nodeSize.y / 2);
        }

        //Store the center position and size based on the current cave
        zoomCaveCamera = nodePosition;
        zoomSizeCamera = Mathf.Max(((zoomMaxX - zoomMinX) / 2.0f) / cam.aspect, (zoomMaxY - zoomMinY) / 2.0f);
    }

    //Get the size for zooming into an individual node
    void getNodeZoomSize()
    {
        nodeZoomSize = root.GetComponent<Renderer>().bounds.size.y;
        nodeZoomSize += 2 * cameraMargin;
    }

    //Change camera position and size based on previously obtained 
    //minimum and maximum x and y values for current and directly connected caves
    void setZoomCamera()
    {
        transform.position = zoomCaveCamera;
        cam.orthographicSize = zoomSizeCamera;
    }

    //If player changes index, expect it to use a function call
    //Change zoom camera position as index changes
    //Get expected time for the player to move
    public void changePlayerIndex(int index, float expectedTime)
    {
        getCaveZoomBounds(index);
        //If we are not in full view, store the current camera size and distance to next cave
        //in order to smoothly move camera
        if (!cameraFullScreen && !nodeZoomCamera)
        {
            setCurrentAndTargets();
            expectedTimeChange = expectedTime;
            elapsedTimeChange = 0;
            elapsedTimeChange += Time.deltaTime;
        }
    }
}
