using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour
{
  private FPSPlayerController playerMovement;
  private float baseSpeed;
  private float speedReduce = 0.3f;
  public GameObject torch;
  public bool TorchOn = true;
  //TODO make guard walk slower when torch is on.
  private bool IsGuard = false;

  void Start()
  {
    playerMovement = GetComponent<FPSPlayerController>();
    baseSpeed = playerMovement.movementSpeed;

  }


  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      TorchOn = !TorchOn;
      torch.SetActive(TorchOn);
    }

    if (IsGuard)
    {
      if (TorchOn)
      {
        playerMovement.movementSpeed = playerMovement.movementSpeed * speedReduce;
      }
      else
      {
        playerMovement.movementSpeed = baseSpeed;
      }
    }
  }
  
}
