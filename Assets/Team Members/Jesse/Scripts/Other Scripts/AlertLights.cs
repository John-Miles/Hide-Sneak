using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLights : MonoBehaviour
{
    public GameObject redLight, blueLight;
    public float strobeTime, endTime;
    public bool flashing;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AlertLightFlash());
        flashing = true;
        StartCoroutine(StrobeLights());
    }

    IEnumerator StrobeLights()
    {
        while (flashing == true)
        {
            redLight.GetComponent<Light>().intensity = 10;
            Debug.Log("RedLight On");

            blueLight.GetComponent<Light>().intensity = 0;
            Debug.Log("BlueLight Off");

            yield return new WaitForSeconds(strobeTime);

            redLight.GetComponent<Light>().intensity = 0;
            Debug.Log("redlight off");

            blueLight.GetComponent<Light>().intensity = 10;
            Debug.Log("bluelight on");

            yield return new WaitForSeconds(strobeTime);
        }
    }

    IEnumerator AlertLightFlash()
    {
        yield return new WaitForSeconds(endTime);
        flashing = false;
        redLight.SetActive(false);
        blueLight.SetActive(false);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}