using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIWin : MonoBehaviour
{

    public GameObject UIWin;
    public GameObject BlurBkg;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        UIWin.SetActive(false);
        BlurBkg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == ("Thief"))
        {
            UIWin.SetActive(true);
            BlurBkg.SetActive(true);
            active = true;
        }

    }

    void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == ("Thief"))
        {
            UIWin.SetActive(false);
            BlurBkg.SetActive(false);
            active = false;
        }
    }
}
