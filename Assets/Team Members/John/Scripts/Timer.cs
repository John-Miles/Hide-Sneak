using System;
using System.Collections;
using System.Collections.Generic;
using John;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    public GameManager gm;
    [SyncVar] 
    public float roundTimer;

    public float gameLength;
    public string roundText;
    [SyncVar]
    public bool startCount;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        
    }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public void Update()
    {   
        //formatting timer in minutes and seconds
        int minutes = Mathf.FloorToInt(roundTimer / 60F);
        int seconds = Mathf.FloorToInt(roundTimer - minutes * 60);
        roundText = string.Format("{0:0}:{1:00}", minutes, seconds);
        
        
        
        //able to delay the timer for the game
        if (startCount)
        {
            roundTimer = (roundTimer - Time.deltaTime);
        }
        //when the timer hits zero, stop counting and call the EndRound event in Game Manager
        if (roundTimer <= -0.1f)
        {
            CmdTimeExpired();
            startCount = false;
            roundTimer = default;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdTimeExpired()
    {
        gm.RPCTimeExpired();
    }
    
    
    //start the countdown
    [Command(requiresAuthority = false)]
    public void CmdStartRound()
    {
        RpcStartRound();
    }

    [ClientRpc]
    public void RpcStartRound()
    {
        startCount = true;
        roundTimer = gameLength;  
    }

   
}
