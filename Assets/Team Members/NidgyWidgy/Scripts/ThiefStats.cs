using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefStats : MonoBehaviour
{ 
   [Range(0f,100f)]
   public float detectAmount;
   
   // TODO detection amount increase on detected, decrease out of detection.
   // TODO max detection amount causes game over, clamp between 0 and max amount.

   public ThiefStats(float amount)
   {
      this.detectAmount = amount;
   }
   
  
   
}
