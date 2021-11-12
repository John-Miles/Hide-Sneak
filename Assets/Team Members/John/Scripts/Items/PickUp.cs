using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : NetworkBehaviour
{
    public float range;
    private ItemManager im;

    public override void OnStartAuthority()
    {
        enabled = true;
        base.OnStartAuthority();
    }

    private void Awake()
    {
        im = FindObjectOfType<ItemManager>();
    }

    //use raycast to target item in scene
    //call pick up event on itemManager
    //pass item reference to itemManager to add ti collected items
    void Update()
    {
        Client();
    }

    void Client()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdPickUp();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdPickUp()
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
                im.RpcItemUpdate(hit.collider.gameObject);
                //im.ItemRemove(hit.collider.gameObject);
            }
        }
    }
   // 
   // [ClientRpc]
   // void RpcItemHit(RaycastHit hit)
   // {
   //     Debug.Log( hit.collider.gameObject.name + " has been collected!");
   //     var item = hit.collider.gameObject;
   //     im.ItemUpdate(item);
   //     im.ItemRemove(item);
   // }
   //
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward)  * range);
    }
}
