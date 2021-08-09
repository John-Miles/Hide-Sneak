using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action StartGame;
    public event Action EndGame;
    public event Action Escape;
    public event Action Caught;
    public event Action TimedOut;

    public List<GameObject> thievesInScene = new List<GameObject>();
    public List<GameObject> guardsInScene = new List<GameObject>();
    public List<GameObject> targetItemsInScene = new List<GameObject>();

    private List<GameObject> escaped = new List<GameObject>();
    private List<GameObject> caught = new List<GameObject>();
    private List<GameObject> timedOut = new List<GameObject>();


    private bool roundOver = false;

    void Start()
    {
        StartRound();

    }

    public void StartRound()
    {
        ListReset();
        StartGame?.Invoke();
        ItemCheck();
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

    public void ItemCheck()
    {
        targetItemsInScene.Clear();
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Item"))
        {
            targetItemsInScene.Add(i);
        }

        if (targetItemsInScene.Count <= 0)
        {
            //Code here checks items are all collected.
        }

        Debug.Log("There are " + targetItemsInScene.Count + " items in the scene");
    }

    public void EndRound()
    {
        EndGame?.Invoke();
        
        Debug.Log("The round is over");
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


    /*
    old Update code related to item pick up
     public GameObject[] targetItems;
    public List<GameObject> collectedList;
    public GameObject greenSquare;
    public bool gameBegins = false;
    
    
    targetItems = GameObject.FindGameObjectsWithTag("Item");

        if (targetItems.Length <= 0)
        {
            if (!roundOver)
            {
                greenSquare.SetActive(true);
                EndRound();
                roundOver = true;
            }
        }
    
    */
}