using System;
using System.Collections;
using Mirror;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThiefUI : NetworkBehaviour
{
    [Header("UI Systems")]
    public GameObject UI;
    public GameObject WorldUI;
    [Header("In Game HUD")]
    public GameObject GameplayHUD;
    public GameObject VictoryHUD;
    public GameObject DefeatHUD;
    public Material defeatMat;
    public Material victoryMat;
    public GameObject countdownBacker;
    public GameObject DrawHUD;
    public Image NVGImage;
    public Sprite NVGOn;
    public Sprite NVGOff;
    [Header("Sliders")]
    public Slider detectSlider;
    public Slider escapeSlider;
    public AudioSource source;
    public AudioClip[] clips;

    
    [Header("Texts")]
    public Text objText;
    public Text timerText;
    public Text countdownText;
    public Text collectText;
    public Text itemText;
    public TMP_Text winResultText;
    public TMP_Text defeatResultText;
    public TMP_Text drawResultText;
    //public TMP_Text waitingText;
    
    int inputDelay;
    
    //MANAGERS
    GameManager gm;
    Timer timer;
    ItemManager im;
    ThiefStatistics _stats;
    private ThiefWorldStats _worldStats;
    [SerializeField] private FPSPlayerController controls;

    public bool countdownRunning = false;
   
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
        UI.SetActive(true);
        WorldUI.SetActive(true);
    }
    private void Awake()
    {
        //collecting reference to managers in the scene
        gm = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();
        im = FindObjectOfType<ItemManager>();
        _stats = GetComponent<ThiefStatistics>();
        _worldStats = GetComponent<ThiefWorldStats>();
        
        inputDelay = gm.preMatchCountdown;
        escapeSlider.maxValue = _stats.maxEscape;
        detectSlider.maxValue = _stats.maxDetect;
        
        
    }
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = (timer.roundText);
        detectSlider.value = _stats.detectValue;
        escapeSlider.value = _stats.escapeValue;
        
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
        objText.text = "";
        collectText.text = "";
        yield return new WaitForSeconds(.5f);
        countdownBacker.SetActive(true);
        objText.text = "Sneak Around and Collect " + im.requiredCount + "\n Items Before The Guards Catch You!";
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
    
    public void ToggleUI(bool TorchOn)
    {
        if (TorchOn)
        {
            NVGImage.sprite = NVGOn;
        }
        else
        {
            NVGImage.sprite = NVGOff;
        }
    }

    public IEnumerator EscapeSet()
    {
        objText.text = "Find A Way Out Before The Guards Find You!";
        yield return new WaitForSeconds(10f);
        objText.text = "";
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

    public void ItemUpdate(int collected, int required)
    {
        itemText.text = "Items: " + collected + " / " + required;
    }

    public void HideGameHUD()
    {
        GameplayHUD.SetActive(false);
    }

    public void ShowEscape()
    {
        escapeSlider.gameObject.SetActive(true);
        
    }

    [Command(requiresAuthority = false)]
    public void CmdShowWorld()
    {
        RpcShowWorldDetect();
    }

    [ClientRpc]
    public void RpcShowWorldDetect()
    {
        
    }
    

    public void ShowDetect()
    {
        detectSlider.gameObject.SetActive(true);
        CmdShowWorld();
    }

    public void HideDetect()
    {
        detectSlider.gameObject.SetActive(false);
        
    }

    public void HideEscape()
    {
        escapeSlider.gameObject.SetActive(false);
        
    }

    public void WaitingEscaped(string description)
    {
        objText.text = description;
        detectSlider.gameObject.SetActive(false);
        escapeSlider.gameObject.SetActive(false);
       
        itemText.gameObject.SetActive(false);

    }

    public void WaitingCaught(string description)
    {
        objText.text = description;
        detectSlider.gameObject.SetActive(false);
        escapeSlider.gameObject.SetActive(false);
        
        itemText.gameObject.SetActive(false);
        
    }

    public IEnumerator OtherCaught()
    {
        objText.text = "The Guards Found A Thief! \n Escape While You Still Can!";
        yield return new WaitForSeconds(5f);
        objText.text = "";
    }
    
    public IEnumerator YouEscaped()
    {
        objText.text = "You Have Escaped!";
        detectSlider.gameObject.SetActive(false);
        escapeSlider.gameObject.SetActive(false);
       
        itemText.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
    }

    public IEnumerator OtherEscaped()
    {
        objText.text = "Your Teammate Has Escaped!\n Join Them Before It's Too Late!";
        yield return new WaitForSeconds(5f);
        objText.text = "";
    }

        public void Loss(string description)
    {
        DefeatHUD.SetActive(true);
        source.clip = clips[1];
        source.Play();
        defeatResultText.text = description;
    }

    public void Win(string description)
    {
        VictoryHUD.SetActive(true);
        source.clip = clips[0];
        source.Play();
        winResultText.text = description;
    }

    public void Draw(string description)
    {
        DrawHUD.SetActive(true);
        source.clip = clips[1];
        source.Play();
        drawResultText.text = description;
    }
    
}
