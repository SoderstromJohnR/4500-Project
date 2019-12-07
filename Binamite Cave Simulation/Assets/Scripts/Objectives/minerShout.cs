using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minerShout : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "OtherMiner";
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
