using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool RandomRole;

    public event Action StartGame;
    public event Action EndGame;

    public GameObject[] targetItems;
    public List<GameObject> collectedList;
    public GameObject greenSquare;
    public bool gameBegins = false;

    private bool roundOver = false;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
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
    }

    public void BeginGame()
    {
        gameBegins = true;
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
}