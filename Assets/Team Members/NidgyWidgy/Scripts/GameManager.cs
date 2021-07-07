using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 private bool RandomRole;

 public event Action StartGame;

 public void StartRound()
 {
   StartGame?.Invoke(); 
 }
 
}
