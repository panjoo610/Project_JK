using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour {

    public Transform StatusParent;
    public GameObject statusUI;
    StatusSlot[] slots;

    private void Awake()
    {
        slots = StatusParent.GetComponentsInChildren<StatusSlot>();
    }
    // Use this for initialization
    void Start()
    {
        //inventory = InventoryManager.instance;
        //inventory.onItemChangedCallBack += UpdateUI;
        AllUpdateUI();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    AllUpdateUI();
    //}

    void AllUpdateUI() //강제적으로 위치를 옮겨줌
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateUI();
        }
    }
}
