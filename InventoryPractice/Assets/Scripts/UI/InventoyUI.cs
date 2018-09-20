using UnityEngine;

public class InventoyUI : MonoBehaviour {

    public Transform itemParent;

    public GameObject inventotyUI;

    InventoryManager inventory;

    InventorySlot[] slots;

    ItemStats[] StatsPanel;
    private void Awake()
    {
        slots = itemParent.GetComponentsInChildren<InventorySlot>();
    }
    // Use this for initialization
    void Start ()
    {
         inventory = InventoryManager.instance;
         inventory.onItemChangedCallBack += UpdateUI;
         UpdateUI();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventotyUI.SetActive(!inventotyUI.activeSelf);

            StatsPanel = inventotyUI.GetComponentsInChildren<ItemStats>();
            for (int i = 0; i < StatsPanel.Length; i++)
            {
                Destroy(StatsPanel[i].gameObject);
            }
        }      
	}

    void UpdateUI() //강제적으로 위치를 옮겨줌
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
