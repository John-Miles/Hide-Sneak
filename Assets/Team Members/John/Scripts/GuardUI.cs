using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GuardUI : NetworkBehaviour
{
    //TEXT VAARIABLES
    public GameObject UI;
    public Text objText;
    public Text timerText;
    public Text countdownText;
    
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
    

    private void Awake()
    {
        //collecting references to managers in the scene
        gm = FindObjectOfType<GameManager>();
        im = FindObjectOfType<ItemManager>();
        timer = FindObjectOfType<Timer>();
        StartCoroutine(MissionSet());
    }
    
    public IEnumerator MissionSet()
    { 
        GetComponent<FPSPlayerController>().enabled = false;
        yield return new WaitForSeconds(1f);
        objText.text = "Stop The Thieves From Collecting All " + im.requiredItems.Count + " Items and Escaping! ";
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
        objText.text = "The thieves are escaping! stop them before its too late!";
        yield return new WaitForSeconds(10f);
        objText.text = "";
    }
    
    

    // Update is called once per frame
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = timer.roundText;

        if (Input.GetKeyDown(KeyCode.F))
        {
            //flashlightSprite.sprite = flashlightOn;
        }
    }
}
