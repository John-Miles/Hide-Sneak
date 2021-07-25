using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetItem : MonoBehaviour
{
    public bool isHighlighted;

    void Awake()
    {
        isHighlighted = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var outline = gameObject.GetComponent<Outline>();
        if (isHighlighted == true)
        {
            outline.OutlineWidth = 5f;
        }
        else
        {
            outline.OutlineWidth = 0f;
        }
    }
}
