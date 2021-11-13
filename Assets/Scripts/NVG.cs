using System;
using Mirror;
using UnityEngine;

namespace Team_Members.NidgyWidgy.Scripts
{
  public class NVG : NetworkBehaviour
  {
    public GameObject nvg;
    public bool nvgOn = true;
    public ThiefUI ui;

    public override void OnStartAuthority()
    {
      base.OnStartAuthority();
      enabled = true;
      nvgOn = nvg.activeSelf;
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
        nvg.SetActive(nvgOn);
        nvgOn = !nvgOn;
        ui.ToggleUI(nvgOn);
    }
    
    //Server Code
    [Command(requiresAuthority = false)]
    void CmdRequestToggle()
    {
      RpcToggle();
    }
    
    
  }
}
