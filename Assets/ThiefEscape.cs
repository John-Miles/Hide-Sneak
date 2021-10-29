using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThiefEscape : NetworkBehaviour
{
    //MANAGERS
    public GameManager gm;
    
    //VARIABLES
    public float escapeValue;
    public float maxEscape;
    
    //REFERENCES
    public GameObject escapeHUD;
    public bool inEscape;
    public Transform escapePos;

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

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EscapePoint"))
        {
            Debug.Log("Player entered escape zone");
            inEscape = true;
            StartCoroutine(Escaping());
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited escape zone");
        StopCoroutine(Escaping());
        escapeHUD.SetActive(false);
        escapeValue = 1;
        inEscape = false;
    }

    public IEnumerator Escaping()
    {
        escapeValue = 1;
        escapeHUD.SetActive(true);
        Debug.Log("Player escaping");
        while (inEscape)
        {
            escapeValue++;
            if (escapeValue >= maxEscape)
            {
                transform.position = gm.escapePos.position;
                gm.CmdEscaped(gameObject);
                Debug.Log("Player has escaped!");
                StopAllCoroutines();
                enabled = false;
                
                yield return null;
            } 
            Debug.Log("More Escape Needed!");
            yield return new WaitForSeconds(.5f);
        }
        Debug.Log("unexpected exit");
        yield return null;
    }

}
