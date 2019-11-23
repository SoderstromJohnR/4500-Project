using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject root;
    public float cameraMargin;

    private float fullSizeCamera;
    private Vector3 fullCaveCamera;
    private float zoomSizeCamera;
    private Vector3 zoomCaveCamera;

    private float currentSize;
    private float distance;
    private float startMoveDistance;
    private float elapsedChangeZoom;
    private GameObject currentCave;
    private bool nodesMade = false;
    private bool cameraFullScreen = true;
    private bool cameraZoomBig = true;
    private bool changingZoom = false;
    private List<int> nodeIndices;
    private Vector3 offset;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        currentCave = root;
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
            nodesMade = true;
        }

        //Change camera mode on input
        if (Input.GetKeyDown(KeyCode.M) && !changingZoom)
        {
            changingZoom = true;
            //Change camera setting, then shift camera based on it
            cameraFullScreen = !cameraFullScreen;
            if (cameraFullScreen)
            {
                //setFullCamera();
            }
            else
            {
                //setZoomCamera();
            }
        }
        
        //Smooth changes from full cave network to zoomed in
        if (changingZoom)
        {
            elapsedChangeZoom += Time.deltaTime;
            if (cameraFullScreen)
            {
                cam.orthographicSize = Mathf.SmoothStep(zoomSizeCamera, fullSizeCamera, elapsedChangeZoom);
                float xCam = Mathf.SmoothStep(zoomCaveCamera.x, fullCaveCamera.x, elapsedChangeZoom);
                float yCam = Mathf.SmoothStep(zoomCaveCamera.y, fullCaveCamera.y, elapsedChangeZoom);
                transform.position = new Vector3(xCam, yCam, transform.position.z);
            }
            else
            {
                cam.orthographicSize = Mathf.SmoothStep(fullSizeCamera, zoomSizeCamera, elapsedChangeZoom);
                float xCam = Mathf.SmoothStep(fullCaveCamera.x, zoomCaveCamera.x, elapsedChangeZoom);
                float yCam = Mathf.SmoothStep(fullCaveCamera.y, zoomCaveCamera.y, elapsedChangeZoom);
                transform.position = new Vector3(xCam, yCam, transform.position.z);
            }
            if (elapsedChangeZoom > 1.0f)
            {
                changingZoom = false;
                elapsedChangeZoom = 0;
            }
        }
        
        //If we're in zoomed mode, move the camera as the player changes cave
        //Still need to try to improve the smoothness, but it works at least
        if (!cameraFullScreen && (transform.position.x != currentCave.transform.position.x))
        {
            //Find current distance from player to center of cave
            distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - currentCave.transform.position.x, 2) + Mathf.Pow(transform.position.y - currentCave.transform.position.y, 2));
            //Get percent (float from 0 to 1) that increases the closer we get to the center of the cave
            float zoomShift = (startMoveDistance - distance) / startMoveDistance;
            cam.orthographicSize = Mathf.SmoothStep(currentSize, zoomSizeCamera, zoomShift);
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    //Get the maximum and minimum x and y coordinates for the full cave network camera
    //Then store into a class variable
    void getCaveFullBounds()
    {
        Vector3 nodeSize = root.GetComponent<Renderer>().bounds.size;
        nodeIndices = root.GetComponent<johnRootController>().getNodeIndices();
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

    //Change camera position and size based on previously obtained 
    //minimum and maximum x and y values for current and directly connected caves
    void setZoomCamera()
    {
        transform.position = zoomCaveCamera;
        cam.orthographicSize = zoomSizeCamera;
    }

    //If player changes index, expect it to use a function call
    //Change zoom camera position as index changes
    public void changePlayerIndex(int index)
    {
        //Change the current cave to the new one for size and position changes
        if (index == 1)
        {
            currentCave = root;
        }
        else
        {
            currentCave = root.GetComponent<johnRootController>().findObject(index);
        }
        getCaveZoomBounds(index);

        //If we are not in full view, store the current camera size and distance to next cave
        //in order to smoothly move camera
        if (!cameraFullScreen)
        {
            currentSize = cam.orthographicSize;
            startMoveDistance = Mathf.Sqrt(Mathf.Pow(transform.position.x - currentCave.transform.position.x, 2) + Mathf.Pow(transform.position.y - currentCave.transform.position.y, 2));
        }
    }
}
