using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
  private GameObject player;
  public GameObject torch;
  public bool TorchOn = true;

  void Start()
  {
    player = this.gameObject;

  }


  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      TorchOn = !TorchOn;
      torch.SetActive(TorchOn);
    }
  }
  
}
