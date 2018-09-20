using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    #region Sigleton
    public static InventoryManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one instance of inventoyu found!");
            return;
        }
        instance = this;
    }
    #endregion
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    public int space = 20;

    public List<Item> items = new List<Item>();


    public SaveInventory saveInventory;

    public void Setting()
    {
        int count = saveInventory.items.Count;
        for (int i = 0; i < count; i++)
        {
            items.Add(saveInventory.items[i]);
        }

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

    public bool Add(Item item)
    {
        if (!item.isDefalutItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }

            items.Add(item);
            saveInventory.items.Add(item);
            saveInventory.SaveItemListByJson();

            if (onItemChangedCallBack != null)
                 onItemChangedCallBack.Invoke();
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }
}
