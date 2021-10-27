using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    //REFERENCES
    public GameObject redLights;
    public GameObject blueLights;
    //VARIABLES
    public float switchDelay;
    public float switchLoops;

    private void Awake()
    {
        StartCoroutine(Alert());
    }

    IEnumerator Alert()
    {
        //play sound effect
        
        //toggle lights
        for (int i = 0; i < switchLoops; i++)
        {
            redLights.SetActive(true);
            blueLights.SetActive(false);
            yield return new WaitForSeconds(switchDelay);
            redLights.SetActive(false);
            blueLights.SetActive(true);
            yield return new WaitForSeconds(switchDelay);  
        }

        Destroy(gameObject);
    }
}
