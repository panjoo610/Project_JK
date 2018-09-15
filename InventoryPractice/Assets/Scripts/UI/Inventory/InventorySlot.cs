﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    Item item, preItem;
    Equipment EquipmentItem;

    public Image icon;
    public Button removeButton;

    public GameObject PanelPrefab;

    ItemStats itemStats;

    private void Start()
    {
       
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        EquipmentItem = (Equipment)newItem;

        icon.sprite = item.icon;
        icon.enabled = true;

        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        InventoryManager.instance.Remove(item);
    }

    public void OnSwapButton()
    {
      
    }

    private void OnMouseDrag()
    {
        Debug.Log("드래그 중 입니다");
        Vector3 mosuePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        this.transform.position = Camera.main.ScreenToWorldPoint(mosuePosition);
    }

    public void OnUseItem(Transform ui)
    {
        if(item != null)
        {
            item.Use();
            OnExitStatPanel(ui);
        }
    }

    public void OnExitStatPanel(Transform ui)
    {
        Destroy(ui.gameObject);
    }

    public void OnItemStatPanel()
    {
        if (item != null && itemStats == null) 
        {
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    Transform childTransForm = c.transform.GetChild(0).transform;
                    Transform Ui = Instantiate(PanelPrefab, childTransForm).transform;
                    
                    itemStats = Ui.GetComponent<ItemStats>();

                    StartCoroutine(ExitCoroutine(Ui));

                    itemStats.ItemName.text = item.name;
                    itemStats.ItemImage.sprite = item.icon;
                    itemStats.ItemArmor.text = EquipmentItem.armorModifier.ToString();
                    itemStats.ItemDamage.text = EquipmentItem.damageModifier.ToString();

                    itemStats.EquimentButtom.onClick.AddListener(() => OnUseItem(Ui));

                    itemStats.ExitButton.onClick.AddListener(() => OnExitStatPanel(Ui));

                    break;
                }
            }
        }
    }
    IEnumerator ExitCoroutine(Transform ui)
    {
        yield return new WaitForSeconds(2.0f);
        if(ui != null)
        OnExitStatPanel(ui);
    }
    

    
}
