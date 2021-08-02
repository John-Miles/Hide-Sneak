using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLookAt : MonoBehaviour
{
    public GameObject targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(targetPoint.transform);
    }
}
