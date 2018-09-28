using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquimentManager : MonoBehaviour
{
    
    #region Singleton
    public static EquimentManager instance;

    private void Awake()
    {
        instance = this;
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

    public Text testText;

    private void Start()
    {
        inventory = InventoryManager.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquiment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefalutItems();

        saveInventory.LoadItemListFromJson();   

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            saveInventory.ResetData();
        }

        if (saveInventory.items != null)
            testText.text = "장착창 : " + saveInventory.equipmentItems.Count.ToString() + "  인벤토리 : " + saveInventory.items.Count.ToString();//테스트
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Unequip(slotIndex);
        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        if (!newItem.isDefalutItem) { saveInventory.equipmentItems.Add(newItem); }
        inventory.Remove(newItem);
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquiment[slotIndex] != null)
        {
            Equipment oldItem = currentEquiment[slotIndex];

            inventory.Add(oldItem);

            Destroy(currentMeshes[slotIndex].gameObject);

            saveInventory.equipmentItems.Remove(oldItem);

            currentEquiment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            if (!oldItem.isDefalutItem)
            {
                FirstEquip(defalutItems[slotIndex]);
            }

            saveInventory.SaveItemListByJson();
            return oldItem;

        }
        saveInventory.SaveItemListByJson();
        return null;
    }

    void EquipDefalutItems()
    {
        foreach (Equipment item in defalutItems)
        {
            FirstEquip(item);
        }
    }

    public void EquipToSaveInven()
    {
        if (saveInventory.equipmentItems == null)
        {
            return;
        }

        for (int i = 0; i < saveInventory.equipmentItems.Count; i++)
        {
            FirstEquip((Equipment)saveInventory.equipmentItems[i]);
        }
        inventory.Setting();
    }

    void FirstEquip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, null);
        }
        if(currentEquiment[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
        }

        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;
    }
}
