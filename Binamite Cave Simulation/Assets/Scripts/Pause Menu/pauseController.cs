using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseController : MonoBehaviour
{
    public int pauseSwitch;
    private int pauseCD;
    // Start is called before the first frame update
    void Start()
    {
        pauseCD = 0;
       
        pauseSwitch = 0;
        // zero is inactive
        // one is active
    }

   
    void Update()
    {
        if (pauseCD > 0)
        {
            pauseCD -= 1;
        }
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject close = GameObject.Find("closeButton");
        //begin pause
        closeController cc = close.GetComponent<closeController>();
        if (Input.GetKeyDown("p") && pauseSwitch == 0 && pauseCD == 0)
        {
            gameObject.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);
            pauseSwitch = 1;
            pauseCD = 15;
           
        }
        //end pause
        
        if (cc.closeSwitch == 1 && pauseCD == 0)
        {
            gameObject.transform.position = new Vector3(100, 100, 0);
            pauseSwitch = 0;
            pauseCD = 15;
        }

        if (Input.GetKeyDown("p") && pauseSwitch == 1 && pauseCD == 0)
        {
            gameObject.transform.position = new Vector3(100, 100, 0);
            pauseSwitch = 0;
            pauseCD = 15;
            
        }     
    }
}
