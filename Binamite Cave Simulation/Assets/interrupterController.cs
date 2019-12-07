using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interrupterController : MonoBehaviour
{
    // Reference to the interrupter prefab. Drag it onto the inspector.
    public GameObject playerInterrupter;
    GameObject interrupterInstance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateInterruption(string Message = "")
    {
        if (GameObject.Find("playerInterrupter") == null)
        {
            Instantiate(playerInterrupter);
        }

        interrupterInstance = GameObject.Find("playerInterrupter");

    }
}
