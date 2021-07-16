using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isOn = false;
    public GameObject Flashlight;


    // Start is called before the first frame update
    void Start()
    {
        Flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOn == false)
            {
                Flashlight.SetActive(true);
                isOn = true;
            }
            
            else if (isOn == true)
            {
                Flashlight.SetActive(false);
                isOn = false;
            }
        }

    }
}
