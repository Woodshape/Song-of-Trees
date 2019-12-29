﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Animator cameraAnim;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        cameraAnim = GetComponent<Animator>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CameraShake()
    {
        cameraAnim.SetTrigger("tShake");
    }
}