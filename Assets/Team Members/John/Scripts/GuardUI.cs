using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GuardUI : NetworkBehaviour
{
    [Header("In Game HUD")]
    public GameObject GameplayHUD;
    public GameObject VictoryHUD;
    public GameObject DefeatHUD;
    public GameObject DrawHUD;
    public GameObject countdownBacker;
    public Image flashImage;
    public Sprite flashlightOff;
    public Sprite flashlightOn;
   
    [Header("Texts")]
    public GameObject UI;
    public Text objText;
    public Text timerText;
    public Text countdownText;
    public Text itemText;
    public TMP_Text winResultText;
    public TMP_Text defeatResultText;
    public TMP_Text drawText;
    int inputDelay;
    public bool countdownRunning = false;
    
    
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
        inputDelay = gm.preMatchCountdown;
    }
    
    public void CheckStart()
    {
        if (!countdownRunning)
        {
            StartCoroutine(MissionSet());
        }
    }
    public IEnumerator MissionSet()
    { 
        countdownRunning = true;
        GetComponent<FPSPlayerController>().canMove = false;
        objText.text = "";
        countdownBacker.SetActive(true);
        yield return new WaitForSeconds(.5f);
        objText.text = "Use Your Flashlight To Catch The Thieves \n Before They Steal " + im.requiredCount + " Items";
        while (inputDelay > 0)
        {
            countdownText.text = inputDelay.ToString();
            yield return new WaitForSeconds(1f);
            inputDelay--;
        }
        GetComponent<FPSPlayerController>().canMove = true;
        countdownText.text = "GO!";
        timer.CmdStartRound();
        objText.text = "";
        yield return new WaitForSeconds(2f);
        countdownText.text = "";
        countdownBacker.SetActive(false);
    }
    
    public IEnumerator EscapeSet()
    {
        objText.text = "The Theives Have Stolen All The Items! \n Catch Them Before They Escape!";
        yield return new WaitForSeconds(10f);
        objText.text = "";
    }
    
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = timer.roundText;
    }

    public void ToggleUI(bool TorchOn)
    {
        if (TorchOn)
        {
            flashImage.sprite = flashlightOn;
        }
        else
        {
            flashImage.sprite = flashlightOff;
        }
    }
    public IEnumerator ItemUpdate(int collected)
    {
        itemText.text = "The Thieves Have Stolen " + collected + " Items!";
        yield return new WaitForSeconds(5f);
        itemText.text = "";
    }

    public void HideGameHUD()
    {
        GameplayHUD.SetActive(false);
    }

    public IEnumerator Caught()
    {
        int remaining = gm.thievesInScene.Count - gm.caught.Count - gm.escaped.Count;
        if (remaining == 1)
        {
            objText.text = "Only 1 Thief Remains! \nFind Them Before They Escape!";
        }
        else
        {
            objText.text = gm.thievesInScene.Count - gm.caught.Count - gm.escaped.Count + " Thieves Left \n Find Them All!";
        }
        
        yield return new WaitForSeconds(5f);
        objText.text = "";
    }

    public IEnumerator SingleEscape()
    {
        int remaining = gm.thievesInScene.Count - gm.caught.Count - gm.escaped.Count;
        if (remaining == 1)
        {
            objText.text = "A Thief Has Escaped! \nFind The Last Thief Before They Escape!";
        }
        else
        {
            objText.text = "A Thief Has Escaped! \nFind The Remaining " + remaining + "Thieves Before It's Too Late";
        }
        
        yield return new WaitForSeconds(5f);
        objText.text = "";
    }
   
    public void Loss(string description)
    {
        DefeatHUD.SetActive(true);        
        defeatResultText.text = description;
    }

    public void Win(string description)
    {
        VictoryHUD.SetActive(true);        
        winResultText.text = description;
    }

    public void Draw(string description)
    {
        DrawHUD.SetActive(true);
        drawText.text = description;
    }
}
