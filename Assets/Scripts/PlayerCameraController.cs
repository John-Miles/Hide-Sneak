using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")] 
    public float minX = -70f;
    public float maxX = 70f;

     float sensitivityX;
     float sensitivityY;
    
    public GameObject cam;
    [SyncVar]
    float rotY = 0f;
    [SyncVar]
    float rotX = 0f;

    private bool inGame;

    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        enabled = true;
        cam.SetActive(true);
        sensitivityX = PlayerPrefs.GetFloat("MouseSensX", 5);
        sensitivityY = PlayerPrefs.GetFloat("MouseSensY", 5);
        inGame = true;
    }

    void Update()
    {
        if (inGame)
        {
            rotY += Input.GetAxis("Mouse X") * sensitivityX;
            rotX += Input.GetAxis("Mouse Y") * sensitivityY;

            rotX = Mathf.Clamp(rotX, minX, maxX);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);

        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Mistake happened here 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inGame = false;
            //bring up menu call here
        }

        if (Cursor.visible && Input.GetMouseButtonDown(0))
        {
            //rename this function to be called on resume button click event
            //hide pause menu UI
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inGame = true;
        }
    }
}
