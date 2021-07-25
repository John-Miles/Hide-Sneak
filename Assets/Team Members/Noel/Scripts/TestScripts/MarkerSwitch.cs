using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSwitch : MonoBehaviour
{

    public GameObject ParentMarker;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        //ParentMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == ("Thief"))
        {
            if (active == true)
            {
                ParentMarker.SetActive(false);
                active = false;
            }
            else if (active == false)
            {
                ParentMarker.SetActive(true);
                active = true;
            }
        }

    }

}
