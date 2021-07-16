using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour
{

 
  public GameObject torch;
  public bool TorchOn = true;


  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      TorchOn = !TorchOn;
      torch.SetActive(TorchOn);
    }
    
  }
  
}
