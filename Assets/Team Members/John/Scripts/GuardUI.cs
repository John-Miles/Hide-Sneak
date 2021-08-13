using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


public class GuardUI : MonoBehaviour
{
    public GameManager gm;
    public RoundManager rm;
    public TMP_Text timerText;

    public Image flashlightSprite;
    //HUD sprite elements
    public Sprite flashlightOn;
    public Sprite flashlightOff;


    private void Awake()
    {
        //collecting references to managers in the scene
        gm = FindObjectOfType<GameManager>();
        rm = FindObjectOfType<RoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //updating timer text with the remaining time
        timerText.text = ("Time Remaining: " + rm.roundText);

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightSprite.sprite = flashlightOn;
        }
    }
}
