using System;
using Mirror;
using UnityEngine;

namespace Team_Members.NidgyWidgy.Scripts
{
  public class Flashlight : NetworkBehaviour
  {
    public GameObject torch;
    public bool TorchOn = true;

    public override void OnStartAuthority()
    {
      base.OnStartAuthority();
      enabled = true;
      TorchOn = true;
    }

    public void Update()
    {
      Client();
    }

    void Client()
    {
      if (Input.GetKeyDown(KeyCode.F))
      {
        CmdRequestToggle();
      }
    }
    [ClientRpc]
    void RpcToggle()
    {
        torch.SetActive(TorchOn);
        TorchOn = !TorchOn;
    }
    
    //Server Code
    [Command(requiresAuthority = false)]
    void CmdRequestToggle()
    {
      RpcToggle();
    }
    
    
  }
}
