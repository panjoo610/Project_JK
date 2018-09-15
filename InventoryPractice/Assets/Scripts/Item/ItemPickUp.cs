using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    void PickUp()
    {
        Debug.Log("아이템을 집었다");
        bool wasPickedUp = InventoryManager.instance.Add(item);

        if(wasPickedUp)
        Destroy(gameObject);
    }

}
