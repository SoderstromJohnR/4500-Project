using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioObjController : MonoBehaviour
{
    private int playOnce;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        playOnce = 0;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("pauseObj");
        pauseController cs = go.GetComponent<pauseController>();
        Slider VolumeSliderGet = GameObject.Find("Slider").GetComponent<Slider>();
        if (cs.pauseSwitch == 0 && playOnce == 0)
        {
            audioSource.Play();
            playOnce = 1;
        }
        if (cs.pauseSwitch == 1)
        {
            audioSource.Pause();
            playOnce = 0;
        }
        if (VolumeSliderGet.value == -20)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
        
    }
}
