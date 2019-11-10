using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minerSearchGame : MonoBehaviour
{
    private GameObject minerShout;
    private GameObject player;
    private Vector3 nodeSize;
    private float distance;
    private Vector3 minerShoutPosition;
    private Vector3 change;
    private int caveIndex;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "OtherMiner";

        minerShout = Resources.Load<GameObject>("minerShout");
        player = GameObject.Find("playerPlaceholder");
        nodeSize = GameObject.FindGameObjectWithTag("Node").GetComponent<Renderer>().bounds.size / 1.5f;
        distance = nodeSize.x * nodeSize.x + nodeSize.y * nodeSize.y;
        distance = Mathf.Pow(distance, 0.5f);
        //Get cave index for miner from its parent cave.
        caveIndex = transform.parent.gameObject.GetComponent<nodeStat>().getIndex();
        Debug.Log("Miner index: " + caveIndex.ToString());
    }

    int getCaveIndex()
    {
        return caveIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //Use Shout key, set to S for now, check if player is moving or not
        if (Input.GetKeyDown(KeyCode.S) && !player.GetComponent<playerSC>().checkMoving())
        {
            //Get current location of the player every time they shout
            int playerIndex = player.GetComponent<playerSC>().getIndex();
            //Check if we are on the left or right subtree of player
            bool isLeft = false;
            int checkIndex = caveIndex;
            
            //Check through parent
            while (checkIndex != 0 && checkIndex != playerIndex)
            {
                if ((checkIndex % 2) == 0)
                {
                    isLeft = true;
                }
                else
                {
                    isLeft = false;
                }
                checkIndex = checkIndex / 2;
            }

            if (checkIndex != playerIndex)
            {
                change = new Vector3(0, distance, 0);
                minerShoutPosition = player.transform.position + change;
                Debug.Log("I'm behind you!");
                Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
            }
            else if (caveIndex != playerIndex)
            {
                if (isLeft)
                {
                    change = new Vector3(-1, -1, 0).normalized * distance;
                    minerShoutPosition = player.transform.position + change;
                    Debug.Log("I'm on the left!");
                    Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
                }
                else
                {
                    change = new Vector3(1, -1, 0).normalized * distance;
                    minerShoutPosition = player.transform.position + change;
                    Debug.Log("I'm on the right!");
                    Instantiate(minerShout, minerShoutPosition, Quaternion.identity);
                }
            }
        }
    }
}
