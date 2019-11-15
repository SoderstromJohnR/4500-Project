using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "OnCave";
        gameObject.transform.localScale = new Vector3(.2f, .2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
