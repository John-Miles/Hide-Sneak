using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

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

   void Update()
   {
      if (thievesInScene.Count == 0)
      {
         foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief"))
         {
            thievesInScene.Add(o);
         }
      }
      
      if (guardsInScene.Count == 0)
      {
         foreach (GameObject a in GameObject.FindGameObjectsWithTag("Guard"))
         {
            guardsInScene.Add(a);
         }
      }

     
   }
   
   
   public void DisplayDetection(float amount, GameObject thief)
   {
      foreach (GameObject t in thievesInScene)
      {
         if (thief == t)
         {
            thief.GetComponentInChildren<Slider>().value = amount;
         }
      }
   }
}
