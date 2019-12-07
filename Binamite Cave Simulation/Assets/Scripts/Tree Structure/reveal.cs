using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reveal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
        //Set the GameObject's Color to black
        nodeImage.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 3)
        {
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to white
            nodeImage.color = Color.white;
        }
    }
}
