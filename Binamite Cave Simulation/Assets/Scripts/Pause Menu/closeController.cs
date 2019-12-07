using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class closeController : MonoBehaviour
{

    public Sprite light;
    public Sprite dark;
    private SpriteRenderer thing;
    public int closeSwitch;
    // Start is called before the first frame update
    void Start()
    {
        closeSwitch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject pause = GameObject.Find("pauseObj");
        //begin pause
        pauseController cc = pause.GetComponent<pauseController>();
        if (cc.pauseSwitch == 0)
        {
            closeSwitch = 0;
        }
    }
    void OnMouseOver()
    {
        thing = gameObject.GetComponent<SpriteRenderer>();
        thing.sprite = dark;
    }

    void OnMouseExit()
    {
        thing = gameObject.GetComponent<SpriteRenderer>();
        thing.sprite = light;
    }
    private void OnMouseUp()
    {
        closeSwitch = 1;
    }

}
