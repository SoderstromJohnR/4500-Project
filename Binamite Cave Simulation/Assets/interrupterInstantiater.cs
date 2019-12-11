using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interrupter : MonoBehaviour
{
    // Reference to the interrupter prefab. Drag it onto the inspector.
    public GameObject playerInterruption;
    GameObject interruptionInstance;

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
            Instantiate(playerInterruption);
        }

        interruptionInstance = GameObject.Find("playerInterruption");

    }
}
