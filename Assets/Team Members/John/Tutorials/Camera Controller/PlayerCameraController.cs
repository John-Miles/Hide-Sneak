using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")] 
    public float minX = -70f;
    public float maxX = 70f;

    public float sensitivity;
    
    public Camera cam;
    [SyncVar]
    float rotY = 0f;
    [SyncVar]
    float rotX = 0f;

    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        enabled = true;
        cam.enabled = true;
    }

    void Update()
    {
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;

        rotX = Mathf.Clamp(rotX, minX, maxX);

        transform.localEulerAngles = new Vector3(0, rotY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Mistake happened here 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Cursor.visible && Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
