using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float range;
    private ItemManager im;

    private void Awake()
    {
        im = FindObjectOfType<ItemManager>();
    }

    //use raycast to target item in scene
    //call pick up event on itemManager
    //pass item reference to itemManager to add ti collected items
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            hit = new RaycastHit();
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            
            if(Physics.Raycast(ray, out hit, range));
            {
                if (hit.collider == null)
                {
                    return;
                }
                
                if (hit.collider.CompareTag("Item"))
                { 
                    ItemHit(hit);
                }
            }
        }
    }

    void ItemHit(RaycastHit hit)
    {
        Debug.Log( hit.collider.gameObject.name + " has been collected!");
        var item = hit.collider.gameObject;
        im.ItemUpdate(item);
        im.ItemRemove(item);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward)  * range);
    }
}
