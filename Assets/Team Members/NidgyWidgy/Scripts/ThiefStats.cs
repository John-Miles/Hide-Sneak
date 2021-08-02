using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class ThiefStats : MonoBehaviour
{ 
   [Range(0f,100f)]
   public float detectAmount;

   public float seenFloat = 1f;
   public float hiddenFloat = 0.5f;

   public bool seen = false;

   private DetectManager detectManager;
   
   // TODO detection amount increase on detected, decrease out of detection.
   // TODO max detection amount causes game over, clamp between 0 and max amount.
   private void OnEnable()
   {
      StartCoroutine(DetectRoutine());
      detectManager = GameObject.Find("DetectionManager")?.GetComponent<DetectManager>();
   }

   IEnumerator DetectRoutine()
   {
      WaitForSeconds wait = new WaitForSeconds(0.1f);

      while (true)
      {
         yield return wait;
         ValueChange();
      }
   }
  public void ValueChange()
   {

      if (detectManager == null)
      {
         detectManager = GameObject.Find("DetectionManager")?.GetComponent<DetectManager>();
      }
      if (seen)
      {
         detectAmount += seenFloat;
       //  Debug.Log(  gameObject.name + "Detect Amount: " + detectAmount);
       if (detectManager)
       {
          detectManager.DisplayDetection(detectAmount, gameObject);
       }

       if (detectAmount >= 100f)
       {
          detectAmount = 100f;
          // thief found. do the thing here!!!
       }
      }

      if (!seen)
      {
         detectAmount -= hiddenFloat;
        // Debug.Log(  gameObject.name + "Detect Amount: " + detectAmount);

         if (detectAmount <= 0f)
         {
            detectAmount = 0f;
         }

         if (detectManager)
         {
            detectManager.DisplayDetection(detectAmount, gameObject);
         }
         
      }
      
      
   }
   
  
   
}
