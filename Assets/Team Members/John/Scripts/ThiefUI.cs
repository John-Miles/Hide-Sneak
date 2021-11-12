using System;
using System.Collections;
using Mirror;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThiefUI : NetworkBehaviour
{
    //TEXT VAARIABLES
    public GameObject UI;
    public Text objText;
    public Text timerText;
    public Text countdownText;
    public Text collectText;
    
    //MISSION MARKERS
   
    
    //SLIDER VARIABLES
    public int inputDelay;
    public float escapeValue;
    public float exposeValue;
    
    //MANAGERS
    GameManager gm;
    Timer timer;
    ItemManager im;
   
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
        UI.SetActive(true);
    }

    public IEnumerator MissionSet()
    { 
        GetComponent<FPSPlayerController>().enabled = false;
        objText.text = "";
        collectText.text = "";
        yield return new WaitForSeconds(.5f);
        objText.text = "Sneak Around and Collect All " + im.requiredItems.Count + "\n Items Before The Guards Catch You!";
        while (inputDelay > 0)
        {
            countdownText.text = inputDelay.ToString();
            yield return new WaitForSeconds(1f);
            inputDelay--;
        }
        GetComponent<FPSPlayerController>().enabled = true;
        countdownText.text = "GO!";
        timer.StartRound();
        objText.text = "";
        yield return new WaitForSeconds(2f);
        countdownText.text = "";
    }

    public IEnumerator EscapeSet()
    {
        objText.text = "Find A Way Out Before The Guards Find You!";
        yield return new WaitForSeconds(10f);
        objText.text = "";
    }
    

    private void Awake()
    {
        //collecting reference to managers in the scene
        gm = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();
        im = FindObjectOfType<ItemManager>();
        StartCoroutine(MissionSet());
    }

  
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = (timer.roundText);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            collectText.text = " Press E to Collect Item";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            collectText.text = "";
        }
    }
}
