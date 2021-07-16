using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBehaviour : MonoBehaviour
{
    public List<GameObject> inLight;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Thief"))
        {
            inLight.Add(other.gameObject);  
        }
             
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Thief"))
        {
            inLight.Remove(other.gameObject);
        }
        
    }
}
