using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitController : MonoBehaviour
{

    public Sprite light;
    public Sprite dark;
    private SpriteRenderer thing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Application.Quit();
    }

}
