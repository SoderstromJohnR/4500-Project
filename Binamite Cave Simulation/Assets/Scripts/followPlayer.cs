using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = new Vector3(5, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //playerSC rc = player.GetComponent<playerSC>();
        transform.position = player.transform.position;

        //create visibility (light to fend off 'fog of war')
        letThereBeLight();
    }
    public void letThereBeLight()
    {
        
        GameObject root = GameObject.FindGameObjectWithTag("Root");
        johnRootController sc = root.GetComponent<johnRootController>();
        int number = 5;

        if (transform.position.y > 2)
        {
            transform.localScale = new Vector3(transform.position.y + 1, transform.position.y + 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
    }
}
