using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour {

    public Transform StatusParent;
    public GameObject statusUI;
    StatusSlot[] slots;
    Scrollbar Scrollbar;

    public Button ExitButton;


    private void Awake()
    {
        slots = StatusParent.GetComponentsInChildren<StatusSlot>();
        Scrollbar = transform.GetComponentInChildren<Scrollbar>();
        Scrollbar.value = 1;
    }
    // Use this for initialization
    void Start()
    {
        //inventory = InventoryManager.instance;
        //inventory.onItemChangedCallBack += UpdateUI;
        AllUpdateUI();
        ExitButton.onClick.AddListener(() => OnExitButton());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            OnExitButton();
        }
    }
    public void OnExitButton()
    {
        gameObject.SetActive(!gameObject.activeSelf);
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
