﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquimentManager : MonoBehaviour
{
    
    #region Singleton
    public static EquimentManager instance;

    private void Awake()
    {
        instance = this;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquiment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];
    }
    #endregion
    public Equipment[] defalutItems;

    public SkinnedMeshRenderer targetMesh;

    Equipment[] currentEquiment;
    SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    InventoryManager inventory;


    public SaveInventory saveInventory;

    private void Start()
    {

        inventory = InventoryManager.instance;

        EquipDefalutItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public void FirstEquip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        saveInventory.items.Remove(newItem);

        if (!newItem.isDefalutItem) { saveInventory.equipmentItems.Add(newItem); }

    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Unequip(slotIndex);

        Equipment oldItem = Unequip(slotIndex);
      
        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        saveInventory.items.Remove(newItem);

        if (!newItem.isDefalutItem) { saveInventory.equipmentItems.Add(newItem); }

    }

    public Equipment Unequip (int slotIndex)
    {
        if(currentEquiment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            Equipment oldItem = currentEquiment[slotIndex];
            inventory.Add(oldItem);
            saveInventory.equipmentItems.Add(oldItem);

            currentEquiment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }

        return null;
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquiment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefalutItems();
    }

    void EquipDefalutItems()
    {
        foreach(Equipment item in defalutItems)
        {
            Equip(item);
        }
    }
}
