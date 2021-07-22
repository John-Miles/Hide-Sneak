using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUILose : MonoBehaviour
{

    public GameObject UILose;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        UILose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == ("Thief"))
        {
            UILose.SetActive(true);
            active = true;
        }

    }

    void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == ("Thief"))
        {
            UILose.SetActive(false);
            active = false;
        }
    }
}
