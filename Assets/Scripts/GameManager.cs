using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool RandomRole;

    public event Action StartGame;
    public event Action EndGame;
    public event Action Escape;

   

    private bool roundOver = false;

    void Start()
    {
        StartRound();
    }
    public void StartRound()
    {
        StartGame?.Invoke();
        Debug.Log("The round has started");
    }

    public void EndRound()
    {
        EndGame?.Invoke();
        Debug.Log("The round is over");
    }

    public void ThiefEscaped()
    {
        Escape?.Invoke();
        Debug.Log("The thief is escaping");
        
        //in game UI needs to subscribe to this function so that it can display the right graphics on screen
        //needs to remove the mouse cursor lock for each player
        //needs to slow the time scale and then lock rigidbodies of the players
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