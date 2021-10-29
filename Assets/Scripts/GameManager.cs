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
        //set UI to time loss UI for all thieves - COMPELTE
        //set UI to time win UI for all guards - COMPLETE
        //disable pick up on all thieves - COMPLETE
        //disable gameplay UI for all thieves/guards - COMPLETE
        //disable movement for all players - COMPLETE
        //UI will include buttons to return to main menu/dc from server
        
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
        //end the game
        //show victory UI for thief by escape
        //show loss UI for guard by escape
        //disable gameplay UI for all thieves/guards
        //UI will include buttons to return to main menu/dc from server
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

    public void CaughtCheck(GameObject thief)
    {
        //add the thief to caught list
        caught.Add(thief);
        if (caught.Count == thievesInScene.Count)
        {
            RpcAllCaught();
        }
    }

    [ClientRpc]
    public void RpcAllCaught()
    {
        //end the game
        //show loss UI to theif by caught
        //show victory UI to guard by capture
        //disable gameplay UI for all thieves/guards
        //UI will include buttons to return to main menu/dc from server
        
        foreach (GameObject thief in thievesInScene)
        {
            var ui = GetComponentInChildren<ThiefUI>();
            thief.transform.Find("UI").Find("Canvas").Find("GameplayHUD").gameObject.SetActive(false);
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
}




/*
public void ItemCheck()
{
    targetItemsInScene.Clear();
    foreach (GameObject i in GameObject.FindGameObjectsWithTag("Item"))
    {
        targetItemsInScene.Add(i);
    }

    if (targetItemsInScene.Count <= 0)
    {
        Debug.LogWarning("All Items Have Been Collected");
    }

    Debug.Log("There are " + targetItemsInScene.Count + " items in the scene");
}



public void ThiefEscaped(GameObject escapingThief)
{
    if (thievesInScene.Count == 0)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
        {
            thievesInScene.Add(o);
        }
    }

    escaped.Add(escapingThief);
    Escape?.Invoke();
    Debug.Log(escapingThief.name + " is escaping!");

    EndCheck();

    //in game UI needs to subscribe to this function so that it can display the right graphics on screen
    //needs to remove the mouse cursor lock for each player
    //needs to slow the time scale and then lock rigidbodies of the players
}

public void ThiefCaught(GameObject caughtThief)
{
    if (thievesInScene.Count == 0)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
        {
            thievesInScene.Add(o);
        }
    }
    
    caught.Add(caughtThief);
    Caught?.Invoke();
    Debug.Log(caughtThief.name + " has been caught!");
}

public void ThiefTimedOut(GameObject TimedOutThief)
{
    if (thievesInScene.Count == 0)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
        {
            thievesInScene.Add(o);
        }
    }
    
    timedOut.Add(TimedOutThief);
    TimedOut?.Invoke();
    Debug.Log(TimedOutThief.name + " has ran out of time!");
}

void EndCheck()
{
    if ((escaped.Count + caught.Count + timedOut.Count) >= thievesInScene.Count)
    {
        Debug.Log("GAME OVER MAN, GAME OVER!");
         EndRound();
    }
}

void ListReset()
{
    escaped.Clear();
    caught.Clear();
    timedOut.Clear();
    thievesInScene.Clear();
    guardsInScene.Clear();
}

*/
