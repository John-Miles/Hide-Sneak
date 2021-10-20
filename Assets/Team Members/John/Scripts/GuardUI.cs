using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GuardUI : MonoBehaviour
{
    public GameManager gm;
    public ItemManager im;
    public Timer timer;
    public Text timerText;

    //public Image flashlightSprite;
    //HUD sprite elements
    //public Sprite flashlightOn;
    //public Sprite flashlightOff;


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
