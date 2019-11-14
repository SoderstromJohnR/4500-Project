using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rootNodeStat : MonoBehaviour
{
    public bool visiting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            //Fetch the SpriteRenderer from the GameObject
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to green
            nodeImage.color = Color.green;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            visiting = true;           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Fetch the SpriteRenderer from the GameObject
            SpriteRenderer nodeImage = GetComponent<SpriteRenderer>();
            //Set the GameObject's Color to blue
            nodeImage.color = Color.blue;
        }
        visiting = false;
    }
}
