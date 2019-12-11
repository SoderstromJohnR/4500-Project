using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    float depthDistance;
    Vector3 startScale;
    GameObject player;
    Vector3 origPosition;

    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = new Vector3(5, 3, 1);
        //Retrieve values from the root controller to calculate range
        johnRootController sc = GameObject.FindGameObjectWithTag("Root").GetComponent<johnRootController>();
        depthDistance = sc.depthDistance;

        //Generate a multiplier for the starting scale based on tree's x and y distances
        //xDim is the x distance from the root node to either child
        float xDim = sc.getBaseXDistance() / 2;
        float multSize = Mathf.Sqrt(xDim * xDim + depthDistance * depthDistance);
        multSize /= GetComponent<Renderer>().bounds.size.x;
        multSize *= 2.5f;// - maxDepth / (maxDepth + 1);

        //Create the initial scale used in letThereBeLight() and set it to the starting light
        startScale = new Vector3(multSize, multSize, 1);
        transform.localScale = startScale;

        //Finally, get the player's location and move there while setting the start point
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position;
        origPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //playerSC rc = player.GetComponent<playerSC>();
        //Keep the light on the player's location
        transform.position = player.transform.position;

        //create visibility (light to fend off 'fog of war')
        letThereBeLight();
    }
    void letThereBeLight()
    {
        //GameObject root = GameObject.FindGameObjectWithTag("Root");
        //johnRootController sc = root.GetComponent<johnRootController>();
        //int number = 5;
        /*
        if (transform.position.y > 2)
        {
            //transform.localScale = new Vector3(transform.position.y + 1, transform.position.y + 1, 1);
        }
        else
        {
            //transform.localScale = new Vector3(3, 3, 1);
        }*/

        //Find the current y distance from the starting point, and scale it so
        //the starting point is 1, depth 1 is 2, depth 2 is 3, and so on.
        //Use this to reduce the scale as the player moves down on the screen while
        //keeping the child nodes inside the light.
        float distance = 1 + ((origPosition.y - transform.position.y) / (depthDistance * 1.3f));
        transform.localScale = startScale / distance;
    }
}
