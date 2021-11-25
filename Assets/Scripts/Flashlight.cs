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
    public AudioSource source;
    public AudioClip[] clips;

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
        if(TorchOn) 
        {
            source.clip = clips[1];
        }
        else
        {
            source.clip = clips[0];
        }
        source.Play();
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
