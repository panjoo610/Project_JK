using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour {

    public Transform StatusParent;
    public GameObject statusUI;
    StatusSlot[] slots;
    Scrollbar Scrollbar;

    private void Awake()
    {
        slots = StatusParent.GetComponentsInChildren<StatusSlot>();
        Scrollbar = transform.GetComponentInChildren<Scrollbar>();
        Scrollbar.value = 1;
    }

    void Start()
    {
        AllUpdateUI();
    }

    public void OnExitButton()
    {
        statusUI.SetActive(!statusUI.activeSelf);
    }

    public void AllUpdateUI() 
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateUI();
        }
    }
}
