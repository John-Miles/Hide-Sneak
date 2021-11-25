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
    public AudioSource source;
    public AudioClip[] clips;

    public override void OnStartAuthority()
    {
      base.OnStartAuthority();
      enabled = true;
      nvg.SetActive(true);
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
        if(nvgOn) 
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
