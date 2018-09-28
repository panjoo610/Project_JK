using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquimentSlot : MonoBehaviour
{
    Item item;

    public Image icon;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;

        icon.enabled = false;
    }

    public void UnEquipemtItem(int slotindex)
    {
        if (item.isDefalutItem)
        {
            return;
        }
        EquimentManager.instance.Unequip(slotindex);
    }
}


