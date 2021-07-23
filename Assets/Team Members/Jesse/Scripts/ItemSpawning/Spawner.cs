using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedItem;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(spawnedItem, spawnPoint.position, transform.rotation);
        Debug.Log("Spawner Has Spawned Object");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
