using System.Collections;
using System.Collections.Generic;
using ParrelSync.NonCore;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    public Slider slider;
    private Camera cam;

    // TODO have bar show up on guard's camera.
    void OnEnable()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 barPos = cam.WorldToScreenPoint(transform.position);
        slider.transform.position = barPos;
    }

  

    /*
    public Slider bar;

    public Text text;

    public float maxDetection;
    public float currentDetection;

    public float decreaseRate;
    public float startingDecreaseRate;
    public float increaseRate;

    public bool isPressed;
    public bool detected = false;

    void Start()
    {
        bar.maxValue = maxDetection;
        startingDecreaseRate = decreaseRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed == false)
        {
            currentDetection -= decreaseRate * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isPressed = true;
            currentDetection += increaseRate * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isPressed = false;
        }

        if(currentDetection <= 0)
        {
            currentDetection = 0;
        }

        if(currentDetection >= maxDetection)
        {
            detected = true;
            Detected();
        }

        bar.value = currentDetection;

        if(Input.GetKeyDown(KeyCode.R) && detected == true)
        {
            detected = false;
            currentDetection = 0;
            decreaseRate = startingDecreaseRate;
            text.text = ("");
        }
    }

    void Detected()
    {
        bar.value = bar.maxValue;
        currentDetection = maxDetection;
        decreaseRate = 0;
        text.text = ("Thief Has Been Detected!");
        Debug.LogWarning("Thief Is Detected!");
    } */
}
