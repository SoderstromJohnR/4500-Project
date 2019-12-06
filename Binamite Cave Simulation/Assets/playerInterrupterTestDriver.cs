using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInterrupterTestDriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject interrupter = GameObject.Find("interrupter");
        interrupter.GetComponent<interrupterController>().instantiateInterruption("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
