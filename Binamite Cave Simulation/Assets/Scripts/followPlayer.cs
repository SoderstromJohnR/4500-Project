using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        Light lit = GetComponent<Light>();
        if ((transform.position.y + 3) > 4)
        {
            lit.range = transform.position.y + 4;
        }
    }
}
