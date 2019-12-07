using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseController : MonoBehaviour
{
    public int pauseSwitch;     // Zero if the game is not paused, one otherwise
    private int pauseCountDown; // Counts down frames after pausing before new input registers

    const int NUM_FRAMES_TO_WAIT = 15;

    // Start is called before the first frame update
    void Start()
    {
        pauseCountDown = 0;
        pauseSwitch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseCountDown > 0)
        {
            pauseCountDown -= 1;
        }
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject close = GameObject.Find("closeButton");

        //begin pause
        closeController cc = close.GetComponent<closeController>();
        if (Input.GetKeyDown("p") && pauseSwitch == 0 && pauseCountDown == 0)
        {
            gameObject.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);
            pauseSwitch = 1;
            pauseCountDown = NUM_FRAMES_TO_WAIT;

            // Stops time
            Time.timeScale = 0;
           
        }

        //end pause
        else if (((Input.GetKeyDown("p") && pauseSwitch == 1) || cc.closeSwitch == 1) && pauseCountDown == 0)
        {
            gameObject.transform.position = new Vector3(100, 100, 0);
            pauseSwitch = 0;
            pauseCountDown = NUM_FRAMES_TO_WAIT;

            // Restarts time
            Time.timeScale = 1;
        }

    }
}
