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

    void Start()
    {
        slots = itemParent.GetComponentsInChildren<EquimentSlot>();

        equimentManager = EquimentManager.instance;
        equimentManager.onEquipmentChanged += OnEquipmentChanged;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            inventotyUI.SetActive(!inventotyUI.activeSelf);
        }
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
            case EquipmentSlot.Feet:
                slots[(int)EquipmentSlot.Feet].ClearSlot();
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
            case EquipmentSlot.Feet:
                slots[(int)EquipmentSlot.Feet].AddItem(equipment);
                break;
            default:
                break;
        }
    }
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem == null)
        {
            UnEquipment(oldItem);
        }
        else
        {
            Equipment(newItem);
        }     
    }
}

