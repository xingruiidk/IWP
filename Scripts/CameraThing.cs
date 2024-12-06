using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraThing : MonoBehaviour
{
    public static CameraThing instance;
    public Transform follow;
    public Transform NSEW;
    public float sensitivity = 100f;
    private float yRotation = 0f;
    private float xRotation = 0f;
    public GameObject[] Cameras;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        RotatePlayer();
        CombatCamera();
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;  
        if (Movement.instance.isGrounded)
        {
            xRotation = Mathf.Clamp(xRotation, -20f, 90f);
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, -60f, 90f);
        }

        follow.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        NSEW.rotation = Quaternion.Euler(0, yRotation, 0f);
    }
    public void CombatCamera()
    {
        if (!PlayerAttack.instance.inCombat)
        {
            ActivateCamera(0);
        }
        else
        {
            ActivateCamera(1);
        }
    }
    public void ActivateCamera(int index)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (i == index)
            {
                Cameras[i].SetActive(true);
            }
            else
            {
                Cameras[i].SetActive(false);
            }
        }
    }
}
