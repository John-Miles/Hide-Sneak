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

    public Transform escapePos;
    public Transform caughtPos;
    
    public void Awake()
    {
        NetworkManagerHnS.OnItemReady += RpcPlayerListUpdate;
        
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

        Debug.Log("The round has started");
    }
    
    [ClientRpc]
    public void RPCTimeExpired()
    {
        Debug.Log("The time has expired!");
        foreach (GameObject thief in thievesInScene)
        {
            var ui = GetComponentInChildren<ThiefUI>();
            thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            thief.transform.Find("UI").Find("Canvas").Find("LoseTimeHUD").gameObject.SetActive(true);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = GetComponentInChildren<GuardUI>();
            guard.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            guard.transform.Find("UI").Find("Canvas").Find("WinTimeHUD").gameObject.SetActive(true);
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
            StartCoroutine(thief.GetComponent<ThiefUI>().EscapeSet());
        }

        foreach (var guard in guardsInScene)
        {
            StartCoroutine(guard.GetComponent<GuardUI>().EscapeSet());
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdEscaped(GameObject thief)
    {
        //add the escaping thief to the list of escaped thieves
        escaped.Add(thief);
        Debug.Log("adding thief to escape list");
        thief.transform.Find("UI").Find("Canvas").Find("EscapingHUD").gameObject.SetActive(false);
        thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
        thief.GetComponent<PickUp>().enabled = false;
        thief.GetComponent<FPSPlayerController>().enabled = false;
        if(escaped.Count == thievesInScene.Count)
            { 
                //if all the theives in the scene have escaped, end the game
                CmdAllEscape();
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
            var ui = GetComponentInChildren<ThiefUI>();
            thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            thief.transform.Find("UI").Find("Canvas").Find("WinEscapeHUD").gameObject.SetActive(true);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = GetComponentInChildren<GuardUI>();
            guard.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            guard.transform.Find("UI").Find("Canvas").Find("LoseEscapeHUD").gameObject.SetActive(true);
            guard.GetComponent<FPSPlayerController>().enabled = false;
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdCaught(GameObject thief)
    {
        //add the thief to caught list
        caught.Add(thief);
        Debug.Log("adding thief to caught list");
        thief.transform.Find("UI").Find("Canvas").Find("ExposeHUD").gameObject.SetActive(false);
        thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
        thief.GetComponent<PickUp>().enabled = false;
        thief.GetComponent<FPSPlayerController>().enabled = false;
        
        if (caught.Count == thievesInScene.Count)
        {
            RpcAllCaught();
        }
    }

    [ClientRpc]
    public void RpcAllCaught()
    {
        foreach (GameObject thief in thievesInScene)
        {
            var ui = GetComponentInChildren<ThiefUI>();
            thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            thief.transform.Find("UI").Find("Canvas").Find("ExposeHUD").gameObject.SetActive(false);
            thief.transform.Find("UI").Find("Canvas").Find("LoseCaptureHUD").gameObject.SetActive(true);
            thief.GetComponent<PickUp>().enabled = false;
            thief.GetComponent<FPSPlayerController>().enabled = false;

        }

        foreach (GameObject guard in guardsInScene)
        {
            var ui = GetComponentInChildren<GuardUI>();
            guard.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
            guard.transform.Find("UI").Find("Canvas").Find("WinCaptureHUD").gameObject.SetActive(true);
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