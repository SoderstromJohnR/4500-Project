using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisController : MonoBehaviour
{
    private bool flagDestroy;
    private bool childOfRoot;
    [SerializeField] private bool isLeftDebris;
    private GameObject dynamite;
    private GameObject childDynamite;
    private GameObject player;

    private int tempGamemode = 1;
    private Episode currentEpisode;

    // Start is called before the first frame update
    void Start()
    {
        dynamite = Resources.Load<GameObject>("dynamite");
        GetComponent<SpriteRenderer>().sortingLayerName = "OnCave";
        gameObject.tag = "Debris";
        //gameObject.transform.localScale = new Vector3(.1f, .1f, 1);
        flagDestroy = false;
        player = GameObject.FindGameObjectWithTag("Player");

        currentEpisode = SceneTransitionManager.Instance.currentEpisode;
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.D) && detonationIsAllowed())
        {
            removeDebris();
        }
    }
    
    // Returns true if dynamite is flagged for episode caving2
    // Only true in caving1 where player is in root and not moving
    private bool detonationIsAllowed()
    {
        // Returns true if there is debris to be detonated and the player is stationary in the root node
        if (currentEpisode == Episode.caving1)
        {
            return flagDestroy
            && player.GetComponent<playerSC>().getIndex() == 1
            && !player.GetComponent<playerSC>().checkMoving();
        }
        return flagDestroy;
    }

    //If player clicks on debris, will call to set it to be destroyed
    public void setFlagDestroy()
    {
        flagDestroy = true;
        childDynamite = Instantiate(dynamite, transform);
        Debug.Log("Preparing to destroy");
    }

    //Destroy all debris that is set to go off
    void removeDebris()
    {
        player.GetComponent<playerSC>().incExplosions();

        childDynamite.GetComponent<dynamiteController>().runExplosion();
        GetComponent<SpriteRenderer>().enabled = false;
        if (childOfRoot && isLeftDebris)
        {
            GetComponentInParent<johnRootController>().removeLeftDebris();
        }
        else if (childOfRoot && !isLeftDebris)
        {
            GetComponentInParent<johnRootController>().removeRightDebris();
        }
        else if (isLeftDebris)
        {
            GetComponentInParent<nodeStat>().removeLeftDebris();
        }
        else
        {
            GetComponentInParent<nodeStat>().removeRightDebris();
        }
        Destroy(gameObject, 2.0f);
    }

    //Send a message when the object is destroyed
    void OnDestroy()
    {
        Debug.Log("Debris destroyed");
    }

    public void setChildOfRoot(bool status)
    {
        childOfRoot = status;
    }

    public void setIsLeftDebris(bool status)
    {
        isLeftDebris = status;
    }

    public bool getChildOfRoot()
    {
        return childOfRoot;
    }

    public bool getFlagDestroy()
    {
        return flagDestroy;
    }
}
