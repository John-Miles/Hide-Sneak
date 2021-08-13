using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThiefUI : MonoBehaviour
{
    public GameManager gm;
    public RoundManager rm;
    public TMP_Text timerText;

    public Image NVGSprite;
    //HUD sprite elements
    public Sprite NVGOn;
    public Sprite NVGOff;
    private void Awake()
    {
        //collecting reference to managers in the scene
        gm = FindObjectOfType<GameManager>();
        rm = FindObjectOfType<RoundManager>();
    }

  
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = ("Time Remaining: " + rm.roundText);
    }
}
