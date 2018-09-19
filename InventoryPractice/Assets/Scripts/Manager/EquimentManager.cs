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


        saveInventory.LoadItemListFromJson();

        EquipDefalutItems();
        EquipSaveInven();

   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
        testText.text = saveInventory.items.Count.ToString();
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

        saveInventory.SaveItemListByJson();
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

            //saveInventory.items.Add(oldItem);
            saveInventory.equipmentItems.Remove(oldItem);

            currentEquiment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            saveInventory.SaveItemListByJson();
            return oldItem;
        }
        saveInventory.SaveItemListByJson();
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

    void EquipSaveInven()
    {
        for (int i = 0; i < saveInventory.equipmentItems.Count; i++)
        {
            FirstEquip((Equipment)saveInventory.equipmentItems[i]);
        }
    }

    void FirstEquip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, null);
        }
        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        //saveInventory.items.Remove(newItem);       
    }
}
