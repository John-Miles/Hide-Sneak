using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefPickup : MonoBehaviour
{
    public Camera camera;
    public int itemList;
    private TargetItem itemBeingPickedUp;

    void Start()
    {

    }

    void Update()
    {
        SelectItemBeingPickedUpFromRay();

        if (HasItemTargetted())
        {
            Debug.Log("An Item Is Highlighted");

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemBeingPickedUp.tag == "Item")
                {
                    Destroy(itemBeingPickedUp.gameObject);
                    itemBeingPickedUp = null;
                    itemList++;
                    Debug.Log("Item Picked Up");
                }
            }
        }
    }

    private bool HasItemTargetted()
    {
        return itemBeingPickedUp != null;
    }

    private void SelectItemBeingPickedUpFromRay()
    {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 4f, Color.red);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 4f))
        {
            var hitItem = hitInfo.collider.GetComponent<TargetItem>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
            }
        }
        else
        {
            itemBeingPickedUp = null;
        }
    }
}
