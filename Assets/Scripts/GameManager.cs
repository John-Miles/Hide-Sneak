using System;
using System.Collections;
using System.Collections.Generic;
using John;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
   [SerializeField] private GameObject escapePoints;

    [SyncVar] public List<GameObject> thievesInScene = new List<GameObject>();
    [SyncVar] public List<GameObject> guardsInScene = new List<GameObject>();
    [SyncVar] public List<GameObject> escaped = new List<GameObject>();
    [SyncVar] public List<GameObject> caught = new List<GameObject>();
    [SyncVar] public List<GameObject> exposed = new List<GameObject>();
    [SyncVar] public List<GameObject> players = new List<GameObject>();

    [Header("Game Over Description Texts")]
    [Tooltip("Description text for when a thief loses by a time out")]
    public string timeOutThief;
    [Tooltip("Description text for when a guard wins by a time out")]
    public string timeOutGuard;
    [Tooltip("Description text for when a thief wins by escaping")]
    public string escapeThief;
    [Tooltip("Description text for when a guard loses by allowing all thieves to escape")]
    public string escapeGuard;
    [Tooltip("Description text for when all thieves lose by being caught")]
    public string caughtThief;
    [Tooltip("Description text for when a guard captures all thieves")]
    public string caughtGuard;
    [Tooltip("Description text for when a thief has been caught but there are still other thieves active in the level")]
    public string waitingCaught;
    [Tooltip("Description text for when a thief has escaped but there are still other thieves active in the level")]
    public string waitingEscaped;
    
    

    public Transform escapePos;
    public Transform caughtPos;
    [Tooltip("The amount in seconds to wait before the game starts after loading the level")]
    public int preMatchCountdown;
    
    public void Awake()
    {
        NetworkManagerHnS.OnItemReady += RpcPlayerListUpdate;
        John.NetworkGamePlayerHnS[] player = FindObjectsOfType<NetworkGamePlayerHnS>();
        foreach (var players in player)
        {
            this.players.Add(players.gameObject);
        }
    }
    
    [ClientRpc]
    public void RpcPlayerListUpdate()
    {
        thievesInScene.Clear();
        guardsInScene.Clear();
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
        {
            thievesInScene.Add(o);
            StartCoroutine(o.GetComponent<ThiefUI>().MissionSet());
        }

        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Guard"))
        {
            guardsInScene.Add(a);
            StartCoroutine(a.GetComponent<GuardUI>().MissionSet());
        }
        if (thievesInScene.Count + guardsInScene.Count == players.Count)
        {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
            {
                StartCoroutine(o.GetComponent<ThiefUI>().MissionSet());
            }
            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Guard"))
            {
                StartCoroutine(a.GetComponent<GuardUI>().MissionSet());
            } 
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
            var ui = GetComponent<GuardUI>();
            ui.HideGameHUD();
            ui.Win(caughtGuard);
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