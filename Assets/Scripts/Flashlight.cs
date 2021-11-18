using System;
using Mirror;
using UnityEngine;

namespace Team_Members.NidgyWidgy.Scripts
{
  public class Flashlight : NetworkBehaviour
  {
    public GameObject torch;
    public bool TorchOn = true;
    public GuardUI ui;

    public override void OnStartAuthority()
    {
      base.OnStartAuthority();
      enabled = true;
      TorchOn = torch.activeSelf;
    }
    
    //Client code
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
        ui.ToggleUI(TorchOn);
    }
    
    //Server Code
    [Command(requiresAuthority = false)]
    void CmdRequestToggle()
    {
      RpcToggle();
    }
    
    
  }
}
