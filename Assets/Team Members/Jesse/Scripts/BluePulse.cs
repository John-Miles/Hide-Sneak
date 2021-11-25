using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePulse : MonoBehaviour
{
    public GameObject canvas;
    public bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
        }

        if (isActive == true)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

}
