using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SaveInventory", menuName = "Inventory/SaveInventory")]
public class SaveInventory : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public List<Item> equipmentItems = new List<Item>();



}
