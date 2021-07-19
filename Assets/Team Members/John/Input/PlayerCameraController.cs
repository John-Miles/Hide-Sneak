using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")] 
    [SerializeField] private Transform playerTransform = null;

    [SerializeField] private Camera cam = null;

    [Header("Sensitivity")]
    public float vertSens = 5f;
    public float horiSens = 5f;

    private Controls _controls;

    private Controls Controls
    {
        get
        {
            if (_controls != null) { return _controls;} 
            return _controls = new Controls();
        }
    }
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Cursor.lockState = CursorLockMode.Locked;
        cam.gameObject.SetActive(true);
        enabled = true;
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();


    private void Look(Vector2 lookAxis)
    {
       playerTransform.Rotate(-lookAxis.y * (vertSens * Time.deltaTime),lookAxis.x * (horiSens * Time.deltaTime),0f);

    }
}
