﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamiteController : MonoBehaviour
{
    private Animator explodeAnim;
    // Start is called before the first frame update
    void Start()
    {
        explodeAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void runExplosion()
    {
        explodeAnim.Play("explosion");
    }
}
