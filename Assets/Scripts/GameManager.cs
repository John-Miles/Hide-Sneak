using System;
using System.Collections;
using System.Collections.Generic;
using John;
using Mirror;
using UnityEditor;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    //ADD TO ACTIVE PLAYERS IN THE LOBBY, SIMILARLY TO HOW YOU ARE HANDLING THE ITEMS COUNTS
    //SYNC ACROSS CLIENTS

    public ItemManager im;
    
    [SerializeField] private GameObject escapePoints;

    [SyncVar] public List<GameObject> thievesInScene = new List<GameObject>();
    [SyncVar] public List<GameObject> guardsInScene = new List<GameObject>();
    [SyncVar] public List<GameObject> escaped = new List<GameObject>();
    [SyncVar] public List<GameObject> caught = new List<GameObject>();
    [SyncVar] public List<GameObject> exposed = new List<GameObject>();
    [SyncVar] public List<GameObject> activePlayers = new List<GameObject>();

    [Header("Game Over Description Texts")]
    [Tooltip("Description text for when a thief loses by a time out")]
    [Multiline]
    public string timeOutThief;
    [Tooltip("Description text for when a guard wins by a time out")]
    [Multiline]
    public string timeOutGuard;
    [Tooltip("Description text for when a thief wins by escaping")]
    [Multiline]
    public string escapeThief;
    [Tooltip("Description text for when a guard loses by allowing all thieves to escape")]
    [Multiline]
    public string escapeGuard;
    [Tooltip("Description text for when all thieves lose by being caught")]
    [Multiline]
    public string caughtThief;
    [Tooltip("Description text for when a guard captures all thieves")]
    [Multiline]
    public string caughtGuard;
    [Tooltip("Description text for when a thief has been caught but there are still other thieves active in the level")]
    [Multiline]
    public string waitingCaught;
    [Tooltip("Description text for when a thief has escaped but there are still other thieves active in the level")]
    [Multiline]
    public string waitingEscaped;
    [Tooltip("Description text for both players when a draw occurs")]
    [Multiline]
    public string draw;
    
    public Transform escapePos;
    public Transform caughtPos;
    [Tooltip("The amount in seconds to wait before the game starts after loading the level")]
    public int preMatchCountdown;
    
    public void Awake()
    {
        timeOutGuard = "Back Up Arrived and \n Found  The Thieves";
        NetworkManagerHnS.OnItemReady += RpcPlayerListUpdate;
        im = FindObjectOfType<ItemManager>();
    }

    [Command(requiresAuthority = false)]
    public void CmdRefreshList()
    {
        RpcPlayerListUpdate();
    }

    [Command(requiresAuthority = false)]
    public void CmdAddToActive(GameObject player)
    {
        RpcAddToActive(player);
    }

    [ClientRpc]
    public void RpcAddToActive(GameObject player)
    {
        activePlayers.Remove(player);
        activePlayers.Add(player);
    }
    
    
    [ClientRpc]
    public void RpcPlayerListUpdate()
    {
        thievesInScene.Clear();
        guardsInScene.Clear();
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
        {
            thievesInScene.Add(o);
            
            
            
        }

        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Guard"))
        {
            guardsInScene.Add(a);
            
            
        }

        CmdUpdatePlayerListAndCheck();
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdatePlayerListAndCheck()
    {
        RpcCheckPlayersReady();
    }

    [ClientRpc]
    public void RpcCheckPlayersReady()
    {
        Debug.Log("Checking if players are active");
        if (thievesInScene.Count + guardsInScene.Count == activePlayers.Count)
        {
            foreach (GameObject o in thievesInScene)
            {
                o.GetComponent<ThiefUI>().CheckStart();
            }
            foreach (GameObject a in guardsInScene)
            {
                a.GetComponent<GuardUI>().CheckStart();
            }
            im.CmdRequiredFill();
            
            //This causes a lot of lag... bad idea doing it this way
            //im.SpawnItems();
            
            //this could also cause lag. shall investigate...
            im.CmdCountdownOutline();

        }
        else
        {
            Debug.Log("Still waiting on players to be ready");
            return;
        }
    }
    
    [ClientRpc]
    public void RPCTimeExpired()
    {
        Debug.Log("The time has expired!");
        foreach (GameObject thief in thievesInScene)
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.HideGameHUD();
            ui.Loss(timeOutThief);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = guard.GetComponent<GuardUI>();
            ui.HideGameHUD();
            ui.Win(timeOutGuard);
            guard.GetComponent<FPSPlayerController>().enabled = false;
        }
    }
    ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// WHEN PLAYERS ESCAPE TURN OFF FPS PLAYER CONTROLLER AND MODEL RENDERER
    /// THEN TELEPORT PLAYERS TO SPECTATOR MODE
    ///
    /// WHEN PLAYERS GET CAUGHT TELEPORT THEM TO A POLICE CAR (???)
    ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    
    [ClientRpc]
    public void RpcAllowEscape()
    {
        escapePoints.SetActive(true);
        foreach (var thief in thievesInScene)
        {
            //update the thief UI to notify them to move to escape point
            StartCoroutine(thief.GetComponent<ThiefUI>().EscapeSet());
        }

        foreach (var guard in guardsInScene)
        {
            //updates guard UI to notify them the thieves can escape
            StartCoroutine(guard.GetComponent<GuardUI>().EscapeSet());
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdEscaped(GameObject thief)
    {
        //add the escaping thief to the list of escaped thieves
        escaped.Add(thief);
        if(escaped.Count == thievesInScene.Count)
        { 
            //if all the theives in the scene have escaped, end the game
            CmdAllEscape(); 
        }
        if (thievesInScene.Count == (escaped.Count + caught.Count))
        {
            RpcDraw();
        }
        else
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.WaitingEscaped(waitingEscaped);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;
            foreach (GameObject player in thievesInScene)
            {
                StartCoroutine(player.GetComponent<ThiefUI>().OtherEscaped());
            }
            foreach (GameObject guard in guardsInScene)
            {
                StartCoroutine(guard.GetComponent<GuardUI>().SingleEscape());
            } 
        }
        
        
        
    }

    [Command(requiresAuthority = false)]
    void CmdAllEscape()
    {
        RpcAllEscaped();
    }
    
    [ClientRpc]
    public void RpcAllEscaped()
    {
       Debug.Log("All theives escaped");
        foreach (GameObject thief in thievesInScene)
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.HideGameHUD();
            ui.Win(escapeThief);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = guard.GetComponent<GuardUI>();
            ui.HideGameHUD();
            ui.Loss(escapeGuard);
            guard.GetComponent<FPSPlayerController>().enabled = false;
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdCaught(GameObject thief)
    {
        //add the thief to caught list
        caught.Add(thief);
        
        if (caught.Count == thievesInScene.Count)
        {
            RpcAllCaught();
        }
        else if (thievesInScene.Count == (escaped.Count + caught.Count))
        {
            RpcDraw();
        }
        else
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.HideGameHUD();
            ui.WaitingCaught(waitingCaught);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;
            
            foreach (GameObject player in thievesInScene)
            {
                StartCoroutine(player.GetComponent<ThiefUI>().OtherCaught());
            }

            foreach (var guard in guardsInScene)
            {
                StartCoroutine(guard.GetComponent<GuardUI>().Caught());
            }
            
        }
        
        
        
        
    }

    [ClientRpc]
    public void RpcAllCaught()
    {
        foreach (GameObject thief in thievesInScene)
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.HideGameHUD();
            ui.Loss(caughtThief);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = guard.GetComponent<GuardUI>();
            ui.HideGameHUD();
            ui.Win(caughtGuard);
            guard.GetComponent<FPSPlayerController>().enabled = false;
        }
    }

    [ClientRpc]
    public void RpcDraw()
    {
        foreach (var thief in thievesInScene)
        {
            var ui = thief.GetComponent<ThiefUI>();
            ui.HideGameHUD();
            ui.Draw(draw);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;
        }

        foreach (var guard in guardsInScene)
        {
            var ui = guard.GetComponent<GuardUI>();
            ui.HideGameHUD();
            ui.Draw(draw);
            guard.GetComponent<FPSPlayerController>().enabled = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdExposeUpdate(GameObject thief)
    {
        Debug.Log("Requesting the addition");
        exposed.Add(thief);
        foreach (GameObject player in exposed)
        {
            player.GetComponent<ThiefStatistics>().RpcSetState();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdExposeRemove()
    {
        Debug.Log("Requesting removal of thieves");
        foreach (var player in exposed)
        {
            player.GetComponent<ThiefStatistics>().RpcUnSetState();
        }
        exposed.Clear();
    }
}