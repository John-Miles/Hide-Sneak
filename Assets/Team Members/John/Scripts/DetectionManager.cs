using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DetectionManager : NetworkBehaviour
{
   public List<GameObject> playerList;
   public List<GameObject> guardList;
   public List<GameObject> thiefList;

   [SerializeField] private GameManager gm;

   // Get a reference to all players within the game
   public override void OnStartServer()
   {
      base.OnStartServer();
      
   }

   private void Awake()
   {

      GameObject tempgm = GameObject.FindGameObjectWithTag("GameManager");

      gm = tempgm.GetComponent<GameManager>();
      
      var players = GameObject.FindGameObjectsWithTag("Player");
      foreach (GameObject p in players)
      {
         playerList.Add(p);
      }

      var thieves = GameObject.FindGameObjectsWithTag("Thief");
      foreach (GameObject t in thieves)
      {
         thiefList.Add(t);
      }

      var guards = GameObject.FindGameObjectsWithTag("Guard");
      foreach (GameObject g in guards)
      {
         guardList.Add(g);
      }
   }

 

   //When a thief enters a guards beam, add them to a list to increase their detection

   //when thieves are not inside a beam allow their detection to decrease

   //set floor limit for detection metre

   //call game end event when player has maxed detection


   //extra bonus would be to allow for multiple thieves to exist in the game.
   //this would require removing a thief from the level and calling the game over event when list of thieves is empty.


}
