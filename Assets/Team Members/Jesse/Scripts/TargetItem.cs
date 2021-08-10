using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetItem : MonoBehaviour
{
    public GameObject alertLights;

    public void Destruction()
    {
        gameObject.SetActive(false);
        Instantiate(alertLights, transform.position, Quaternion.identity);
    }
}
