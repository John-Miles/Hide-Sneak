using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThiefUI : NetworkBehaviour
{
    public GameObject UI;
    
    //VARIABLES
    public float escapeValue;
    //MANAGERS
    GameManager gm;
    Timer timer;
    ItemManager im;
    
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
        //collecting reference to managers in the scene
        gm = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();
        im = FindObjectOfType<ItemManager>();
    }

  
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = (timer.roundText);
    }
}
