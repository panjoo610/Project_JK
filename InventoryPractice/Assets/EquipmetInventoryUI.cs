using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmetInventoryUI : MonoBehaviour
{

    public Transform itemParent;

    public GameObject inventotyUI;

    EquimentManager equimentManager;

    [SerializeField]
    EquimentSlot[] slots;

    private void Awake()
    {
        slots = itemParent.GetComponentsInChildren<EquimentSlot>();
    }

    private void Start()
    {
        equimentManager = EquimentManager.instance;
        equimentManager.onEquipmentChanged += OnEquipmentChanged;
    }

    public void OnOff()
    {
        inventotyUI.SetActive(!inventotyUI.activeSelf);
    }

    void UnEquipment(Equipment equipment)
    {
        switch (equipment.equipSlot)
        {
            case EquipmentSlot.Head:
                slots[(int)EquipmentSlot.Head].ClearSlot();
                break;
            case EquipmentSlot.Chest:
                slots[(int)EquipmentSlot.Chest].ClearSlot();
                break;
            case EquipmentSlot.Legs:
                slots[(int)EquipmentSlot.Legs].ClearSlot();
                break;
            case EquipmentSlot.Weapon:
                slots[(int)EquipmentSlot.Weapon].ClearSlot();
                break;
            case EquipmentSlot.Arms:
                slots[(int)EquipmentSlot.Arms].ClearSlot();
                break;
            default:
                break;
        }
    }
    void Equipment(Equipment equipment)
    {
        switch (equipment.equipSlot)
        {
            case EquipmentSlot.Head:
                slots[(int)EquipmentSlot.Head].AddItem(equipment);
                break;
            case EquipmentSlot.Chest:
                slots[(int)EquipmentSlot.Chest].AddItem(equipment);
                break;
            case EquipmentSlot.Legs:
                slots[(int)EquipmentSlot.Legs].AddItem(equipment);
                break;
            case EquipmentSlot.Weapon:
                slots[(int)EquipmentSlot.Weapon].AddItem(equipment);
                break;
            case EquipmentSlot.Arms:
                slots[(int)EquipmentSlot.Arms].AddItem(equipment);
                break;
            default:
                break;
        }
    }
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            Equipment(newItem);        
        }
        else
        {
            UnEquipment(oldItem);
        }     
    }
}

