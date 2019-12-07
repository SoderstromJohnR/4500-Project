using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volumeController : MonoBehaviour
{

    public AudioMixer masterMixer;

    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);
    }
}