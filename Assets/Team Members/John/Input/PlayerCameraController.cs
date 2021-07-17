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
        enabled = true;
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();


    private void Look(Vector2 lookAxis)
    {
       playerTransform.Rotate(0f,lookAxis.x * Time.deltaTime,0f);

    }
}
