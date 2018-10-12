using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    Item item;
    Equipment EquipmentItem;

    public Image icon;
    //public Button removeButton;

    public GameObject PanelPrefab;

    ItemStats itemStats;


    public void AddItem(Item newItem)
    {
        item = newItem;
        EquipmentItem = (Equipment)newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    //public void OnRemoveButton()
    //{
    //   // 
    //}

    //public void OnSwapButton()
    //{
      
    //}

    //private void OnMouseDrag()
    //{
    //    Debug.Log("드래그 중 입니다");
    //    Vector3 mosuePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    //    this.transform.position = Camera.main.ScreenToWorldPoint(mosuePosition);
    //}

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

    public void Sell(Transform ui)
    {
        int temp = PlayerManager.instance.saveInventory.PlayerGold + item.itemPrice;
        InventoryManager.instance.Remove(item);

        PlayerManager.instance.ShowPlayerGold(temp);

        OnExitStatPanel(ui);
    }

    public void OnItemStatPanel()
    {
        if (item != null && itemStats == null) 
        {
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    Transform childTransForm = c.transform.GetChild(1).transform;
                    Transform Ui = Instantiate(PanelPrefab, childTransForm).transform;
                    
                    itemStats = Ui.GetComponent<ItemStats>();

                    StartCoroutine(ExitCoroutine(Ui));

                    itemStats.ItemImage.sprite = item.icon;

                    itemStats.ItemName.text = item.name;                   
                    itemStats.ItemArmor.text = EquipmentItem.armorModifier.ToString();
                    itemStats.ItemDamage.text = EquipmentItem.damageModifier.ToString();

                    itemStats.EquimentButtom.onClick.AddListener(() => OnUseItem(Ui));
                    itemStats.SellButoon.onClick.AddListener(() => Sell(Ui));
                    itemStats.ExitButton.onClick.AddListener(() => OnExitStatPanel(Ui));

                   
                    break;
                }
            }
        }
    }
    IEnumerator ExitCoroutine(Transform ui)
    {
        yield return new WaitForSeconds(2.5f);
        if(ui != null)
        OnExitStatPanel(ui);
    }
    

    
}
