using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoint : MonoBehaviour
{
    public RectTransform prefab;

    private RectTransform waypoint;

    private Transform player;

    private Vector3 offset = new Vector3(0, 4, 0);

    void Start()
    {
        var canvas = GameObject.Find("Waypoints").transform;
        waypoint = Instantiate(prefab, canvas);
        player = GameObject.Find("TestPlayer").transform;
    }

    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
        waypoint.position = screenPos;

        waypoint.gameObject.SetActive(screenPos.z > 0);


    }
}

/*
    
    ///this is an old waypoint script
    ///
    public Image icon;
    public Transform target;
    public Vector3 offset;

    private void Update()
    {
        float minX = icon.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = icon.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            //The target is behind the player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        icon.transform.position = pos;
    }







///////////////////////////////////////
///

private Image iconImg;
    //private Text distanceText;

    public Transform player;
    public Transform target;
    public Camera cam;

    public float closeDistance; //this function will distroy the icon when player is too close

    private void Start()
    {
        iconImg.GetComponent<Image>();
        //distanceText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (target != null)
        {
            GetDistance();
            CheckOnScreen();
        }
    }

    private void GetDistance()
    {
        float dist = Vector3.Distance(player.position, target.position);
        //distanceText.text = dist.ToString("f1") + "m";

        if (dist < closeDistance)
        {
            Destroy(gameObject);
        }
    }

    private void CheckOnScreen()
    {
        float iconF = Vector3.Dot((target.position - cam.transform.position).normalized, cam.transform.forward);

        if (iconF <= 0)
        {
            ToggleUI(false);
        }
        else
        {
            ToggleUI(true);
            transform.position = cam.WorldToScreenPoint(target.position);
        }
    }

    private void ToggleUI(bool _value)
    {
        iconImg.enabled = _value;
        //distanceText.enable = _value;
    }














    */
