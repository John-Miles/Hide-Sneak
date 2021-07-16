using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : NetworkBehaviour
{
    public GameManager gameManager;
    [SyncVar] public float roundTimer;
    public string roundText;
    public Text UIText;
    public bool startCount;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        if (isServer)
        {
            gameManager.StartGame += RPCStartRound;
            roundTimer = 300.0f;
            
        }
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        gameManager.StartGame -= RPCStartRound;
    }

    public void Update()
    {   
        //formatting timer in minutes and seconds
        int minutes = Mathf.FloorToInt(roundTimer / 60F);
        int seconds = Mathf.FloorToInt(roundTimer - minutes * 60);
        roundText = string.Format("{0:0}:{1:00}", minutes, seconds);
        
        //updating timer text with the remaining time
        UIText.text = ("Time Remaining: " + roundText);
        
        //able to delay the timer for the game
        if (startCount)
        {
            roundTimer = (roundTimer - Time.deltaTime);
        }
        //when the timer hits zero, stop counting and call the EndRound event in Game Manager
        if (roundTimer <= -0.1f)
        {
            gameManager.EndRound();
            startCount = false;
            roundTimer = default;
        }
    }
    
    //start the countdown
    [ClientRpc]
    public void RPCStartRound()
    {
        startCount = true;
    }

   
}
