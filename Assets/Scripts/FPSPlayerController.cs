using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class FPSPlayerController : NetworkBehaviour
{
   [SerializeField] private float movementSpeed;
   [SerializeField] private CharacterController controller = null;

   private Vector2 previousInput;

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
      enabled = true;

      Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
      Controls.Player.Move.canceled += ctx => ResetMovement();
      
   }

   [ClientCallback]
   private void OnEnable() => Controls.Enable();

   [ClientCallback]
   private void OnDisable() => Controls.Disable();

   [ClientCallback]
   private void Update() => Move();


   [Client]
   private void SetMovement(Vector2 movement) => previousInput = movement;

   [Client]
   private void ResetMovement() => previousInput = Vector2.zero;

   [Client]
   private void Move()
   {
      Vector3 right = controller.transform.right;
      Vector3 forward = controller.transform.forward;
      right.y = 0;
      forward.y = 0;

      Vector3 movement = Vector3.right.normalized * previousInput.x + forward.normalized * previousInput.y;

      controller.Move(movement * movementSpeed * Time.deltaTime);


   }
}

