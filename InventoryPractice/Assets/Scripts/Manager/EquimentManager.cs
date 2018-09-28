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
        testText.text = "인벤토리 : "+ saveInventory.items.Count.ToString() +"  장착창 : " + saveInventory.equipmentItems.Count.ToString();//테스트
    }

    public void Equip(Equipment newItem)
    {
        inventory.Remove(newItem);

        saveInventory.equipmentItems.Add(newItem);

        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex, newItem);

        Unequip(slotIndex, newItem);
        
        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

    }


    public void NormalEquip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        if(currentEquiment[slotIndex] != null)
        Destroy(currentMeshes[slotIndex].gameObject);

        Equipment oldItem = Unequip(slotIndex, newItem);

        Unequip(slotIndex, newItem);

        currentEquiment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
    }



    public Equipment Unequip (int slotIndex, Equipment newItem)
    {
        Equipment oldItem = currentEquiment[slotIndex];

        if (currentEquiment[slotIndex] != null)
        {
            inventory.Add(oldItem);

            saveInventory.equipmentItems.Remove(oldItem);

            Destroy(currentMeshes[slotIndex].gameObject);          

            currentEquiment[slotIndex] = null;

            NormalEquip(defalutItems[slotIndex]);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
        saveInventory.SaveItemListByJson();
        return oldItem;
    }


    void EquipDefalutItems()
    {     
        foreach(Equipment item in defalutItems)
        {
            NormalEquip(item);
        }
    }

    public void EquipToSaveInven()
    {
        if(saveInventory.equipmentItems == null)
        {
            return;
        }

        for (int i = 0; i < saveInventory.equipmentItems.Count; i++)
        {
            NormalEquip((Equipment)saveInventory.equipmentItems[i]);
        }
        inventory.Setting();
    }
}
