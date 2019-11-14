using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject root;
    public float cameraMargin;

    private float fullMinX;
    private float fullMaxX;
    private float fullMinY;
    private float fullMaxY;
    private float fullSize;
    private int playerIndex;
    private bool nodesMade = false;
    private bool cameraFullScreen = true;
    private List<int> nodeIndices;
    private Vector3 offset;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //offset = transform.position - player.transform.position;

    }

    // LateUpdate runs once every frame after all items have been processed
    void LateUpdate()
    {
        /* Commenting out while I focus on things for this iteration - John
        getCaveFullBounds();
        //transform.position = player.transform.position + offset;
        if (nodesMade)
        {
            getCaveFullBounds();
            nodesMade = true;
        }

        //Change camera mode on input
        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraFullScreen = !cameraFullScreen;
        }
        Vector3 v3 = transform.position;
        //v3.x = (fullMaxX + fullMinX) / 2;
        //v3.y = (fullMaxY - fullMinY) / 2;
        v3.x = Mathf.Clamp(v3.x, fullMinX, fullMaxX);
        v3.y = Mathf.Clamp(v3.y, fullMinY, fullMaxY);
        transform.position = v3;
        Debug.Log(fullMinX.ToString() + ", " + fullMaxX + ", " + fullMinY + ", " + fullMaxY);
        cam.orthographicSize = (fullMaxX - fullMinX) / 3;
        */
    }

    //Get the maximum and minimum x and y coordinates for the full cave network camera
    void getCaveFullBounds()
    {
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
        Debug.Log(actualMaxDepth.ToString());
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
        float topY = root.GetComponent<johnRootController>().findObject(1).transform.position.y;
        float height = actualMaxDepth * depthDistance;
        float xMin = leftX - cameraMargin;
        float yMin = -1 * height;
        float width = rightX - leftX + 2 * cameraMargin;
        Vector2 centerPoint = new Vector2((leftX + rightX) / 2, yMin / 2);
        Vector2 size = new Vector2(width, height);
        //cam.rect = new Rect(centerPoint, size);
        fullMinX = leftX;
        fullMaxX = rightX;
        fullMinY = yMin - cameraMargin;
        fullMaxY = topY + cameraMargin;
        //if (!(newAspectRatio - aspectRatio < .1))
        //cam.rect = new Rect(xMin, yMin, width, height);
        //cam.ResetAspect();
        //cam.rect = new Rect(-10, -5, 20, 7);
    }

    //Get maximum and minimum x and y coordinatse for player and nearest possible 3 nodes
    void getCloseViewBound()
    {
        playerIndex = player.GetComponent<playerSC>().getIndex();
    }

    //If player changes index, expect it to use a function call
    public void changePlayerIndex(int index)
    {
        playerIndex = index;
    }
}
