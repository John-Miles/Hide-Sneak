using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAlert : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnArea;

    void Start()
    {
        spawnArea = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
    }
    public void AlertTest()
    {
        Instantiate(obj, spawnArea, Quaternion.identity);
    }
}
