using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 private bool RandomRole;

 public event Action StartGame;
 public event Action EndGame;



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
