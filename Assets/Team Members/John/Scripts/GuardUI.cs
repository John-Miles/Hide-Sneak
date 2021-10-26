using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GuardUI : NetworkBehaviour
{
    public GameObject UI;
    //MANAGERS
    public GameManager gm;
    public ItemManager im;
    public Timer timer;
    
    //UI ELEMENTS
    public Text timerText;

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
