using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThiefUI : MonoBehaviour
{
    public GameManager gm;
    public Timer rm;
    public Text timerText;

    public Image NVGSprite;
    //HUD sprite elements
    public Sprite NVGOn;
    public Sprite NVGOff;
    private void Awake()
    {
        //collecting reference to managers in the scene
        gm = FindObjectOfType<GameManager>();
        rm = FindObjectOfType<Timer>();
    }

  
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = (rm.roundText);
    }
}
