using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThiefStatistics : NetworkBehaviour
{
    //MANAGERS
    public GameManager gm;
    public ThiefUI ui;
    
    //VARIABLES
    public float detectValue;
    public float maxDetect;
    public float exposeReduce;
    private float _minValue = 0;
    public float escapeValue;
    public float maxEscape;
    public float escapeReduce;
    
    public bool inExpose;
    public bool inEscape;
    public bool running1;
    public bool running2;
    
    //REFERENCES
    private ThiefAudioManager audio;
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
        inEscape = false;
    }
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        audio = GetComponent<ThiefAudioManager>();
        
    }

    [ClientRpc]
    public void RpcSetState()
    {
        inExpose = true;
    }
    
    [ClientRpc]
    public void RpcUnSetState()
    {
        inExpose = false;
    }
    
    

    //CAPTURE FUNCTION
    public void Update()
    {
        if (inExpose && !running1)
        {
            StopCoroutine(DecreaseExposure());
            StartCoroutine(IncreaseExposure());
        }
        if (!inExpose && !running2)
        {
            StopCoroutine(IncreaseExposure());
            StartCoroutine(DecreaseExposure());
        }
        
        
    }

    public IEnumerator IncreaseExposure()
    {
        running1 = true;
        //ui.ShowDetect();
        Debug.Log("Player exposed");
        while (inExpose)
        {
            ui.ShowDetect();
            //increase escape value
            detectValue++;
            //if fully escaped
            if (detectValue >= maxDetect)
            {
                //move the player to spectator spot
                transform.position = gm.caughtPos.position;
                //semd player reference to GM for escaped call
                gm.CmdCaught(gameObject);
                Debug.Log("Player has been caught!");
                //disable this script and stop escape loop
                enabled = false;
                
                yield return null;
            } 
            Debug.Log("More Exposure Needed!");
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitForSeconds(.5f);
        running1 = false;
    }

    public IEnumerator DecreaseExposure()
    {
        running2 = true;
        while (!inExpose && detectValue > _minValue)
        {
            Debug.Log("Cooling down from exposure!");
            detectValue = detectValue - exposeReduce;
            if (detectValue < _minValue)
            {
                Debug.Log("Expose at 0");
                
                yield return null;

            }

            yield return new WaitForSeconds(.5f);
        }
        ui.HideDetect();
        running2 = false;
    }
    
    //ESCAPING FUNCTION
    
    //When a player enters an escape point, begin the escape sequence
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EscapePoint"))
        {
            Debug.Log("Player entered escape zone");
            inEscape = true;
            StartCoroutine(Escaping());
        }
    }
    //when a player exits and escape point, set the cooldown for the escape
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited escape zone");
        StopCoroutine(Escaping());
        inEscape = false;
        Debug.Log("Time to cool down");
        StartCoroutine(EscapeCooldown());
    }
  
    public IEnumerator Escaping()
    {
        while (inEscape)
        {
            //increase escape value
            escapeValue++;
            ui.ShowEscape();
            //if fully escaped
            if (escapeValue >= maxEscape)
            {
                //move the player to spectator spot (needed due to client authority for movement)
                transform.position = gm.escapePos.position;
                //semd player reference to GM for escaped call
                gm.CmdEscaped(gameObject);
                Debug.Log("Player has escaped!");
                StopAllCoroutines();
                //disable this script and stop escape loop
                enabled = false;
                
                yield return null;
            } 
            Debug.Log("More Escape Needed!");
            yield return new WaitForSeconds(.5f);
        }
        yield return null;
    }
    //when the player already has an escape value and steps out of an escape zone
    //they will begin a cooldown until their escape value is 0
    public IEnumerator EscapeCooldown()
    {
        while (!inEscape && escapeValue > _minValue)
        {   
            Debug.Log("Cooling down from escape!");
            escapeValue = escapeValue - escapeReduce; 
            if (escapeValue <= _minValue)
            {
                Debug.Log("Escape at 0");
                
                yield return null;
                
            }
            yield return new WaitForSeconds(.5f);
        }
        ui.HideEscape();
        yield return null;
    }
}
