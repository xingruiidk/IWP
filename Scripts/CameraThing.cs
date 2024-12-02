using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThing : MonoBehaviour
{
    public static CameraThing instance;
    public Transform follow;
    public Transform NSEW;
    public float sensitivity = 100f;
    private float yRotation = 0f;
    private float xRotation = 0f;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        RotatePlayer();
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
}
