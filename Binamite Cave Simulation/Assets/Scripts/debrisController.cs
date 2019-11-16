using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisController : MonoBehaviour
{
    private bool flagDestroy;
    private bool childOfRoot;
    private bool isLeftDebris;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "OnCave";
        gameObject.transform.localScale = new Vector3(.1f, .1f, 1);
        flagDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && flagDestroy)
        {
            destroyWithAnimation();
        }
    }

    //If player clicks on debris, will call to set it to be destroyed
    public void setFlagDestroy()
    {
        flagDestroy = true;
        Debug.Log("Preparing to destroy");
    }

    //Destroy all debris that is set to go off
    public void destroyWithAnimation()
    {
        Destroy(gameObject, 2);
        if (childOfRoot && isLeftDebris)
        {
            transform.GetComponentInParent<johnRootController>().removeLeftDebris();
        }
        else if (childOfRoot && !isLeftDebris)
        {
            transform.GetComponentInParent<johnRootController>().removeRightDebris();
        }
        else if (isLeftDebris)
        {
            transform.GetComponentInParent<nodeStat>().removeLeftDebris();
        }
        else
        {
            transform.GetComponentInParent<nodeStat>().removeLeftDebris();
        }
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
}
