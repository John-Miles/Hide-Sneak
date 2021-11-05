using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights2 : MonoBehaviour
{
    public float endTime, pattern;
    public bool lightsOn;
    public GameObject lookPoint;

    // Start is called before the first frame update
    void Start()
    {
        lightsOn = true;
        StartCoroutine(LightFlash());
        StartCoroutine(LightTimer());
    }

    void Update()
    {
        transform.LookAt(lookPoint.transform);

        if (lightsOn == false)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator LightFlash()
    {
        while (lightsOn == true)
        {
            gameObject.GetComponent<Light>().intensity = 0.5f;

            yield return new WaitForSeconds(pattern);

            gameObject.GetComponent<Light>().intensity = 0f;

            yield return new WaitForSeconds(pattern);
        }
    }

    IEnumerator LightTimer()
    {
        yield return new WaitForSeconds(endTime);
        lightsOn = false;
    }
}
