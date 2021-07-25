using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefPickup : MonoBehaviour
{
    public GameObject Interactable;
    public GameObject craftingUI;
    public Camera camera;
    private TargetItem itemBeingPickedUp;

    private void Start()
    {

    }

    void Update()
    {
        SelectItemBeingPickedUpFromRay();

        if (HasItemTargetted())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemBeingPickedUp.tag == "Item")
                {
                    Destroy(itemBeingPickedUp.gameObject);
                    itemBeingPickedUp = null;
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

                if (itemBeingPickedUp.tag == "Crystal")
                {

                }

                }
                else if (itemBeingPickedUp.tag == "Water")
                {
                    pickUpMessage.text = "(Hold E) Drink Water";
                }
                else if (itemBeingPickedUp.tag == "CraftingTable")
                {
                    pickUpMessage.text = "(E) Open Crafting Menu";
                }

            }

        }
        else
        {
            itemBeingPickedUp = null;
        }
    }
}
