using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DetectManager : NetworkBehaviour
{
   public List<GameObject> thievesInScene = new List<GameObject>();
   public List<GameObject> guardsInScene = new List<GameObject>();

   void OnEnable()
   {
      foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
      {
        thievesInScene.Add(o);
      }

      foreach (GameObject a in GameObject.FindGameObjectsWithTag("Guard"))
      {
         guardsInScene.Add(a);
      }
   }
   
   
   public void DisplayDetection(float amount, GameObject thief)
   {
      Debug.Log("DetectManager on " + thief.name + "detect amount is" + amount.ToString("00"));
   }
}
