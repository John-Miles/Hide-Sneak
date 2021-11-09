using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThiefStatistics : NetworkBehaviour
{
    //MANAGERS
    public GameManager gm;
    
    //VARIABLES
    public float exposeValue;
    public float maxExpose;
    public float exposeReduce;
    public bool inExpose;

    private float _minValue = 0;
    public float escapeValue;
    public float maxEscape;
    public float escapeReduce;
    public bool inEscape;

    public bool running1;
    public bool running2;
    
    //REFERENCES
    public GameObject exposeHUD;
    public GameObject escapeHUD;
    

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
        inEscape = false;
    }
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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
        exposeHUD.SetActive(true);
        Debug.Log("Player exposed");
        while (inExpose)
        {
            //increase escape value
            exposeValue++;
            //if fully escaped
            if (exposeValue >= maxExpose)
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
        while (!inExpose && exposeValue > _minValue)
        {
            Debug.Log("Cooling down from exposure!");
            exposeValue = exposeValue - exposeReduce;
            if (exposeValue < _minValue)
            {
                Debug.Log("Expose at 0");
                exposeHUD.SetActive(false);
                yield return null;

            }

            yield return new WaitForSeconds(.5f);
        }
        running2 = false;
    }
    
    //receive notification of player entering the flashlight of guards
    //set loop for increasing exposure value while constantly checking still in flashlight
    //upon exit of flashlight stop detection
    
    //when exposure is full, call caught function on game manager
    





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
    //when a player exits and escape point, reset the escape
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
        //escapeValue = 1;
        //show the HUD notification
        escapeHUD.SetActive(true);
        Debug.Log("Player escaping");
        while (inEscape)
        {
            //increase escape value
            escapeValue++;
            //if fully escaped
            if (escapeValue >= maxEscape)
            {
                //move the player to spectator spot
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
        Debug.Log("unexpected exit");
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
            if (escapeValue < _minValue)
            {
                Debug.Log("Escape at 0");
                escapeHUD.SetActive(false);
                yield return null;
                
            }
            yield return new WaitForSeconds(.5f);
        }

        yield return null;

    }

}
