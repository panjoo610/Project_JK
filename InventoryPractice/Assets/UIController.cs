using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour {

    public Button StatusBtn, InvenBtn, EquipBtn, StartButton;

    public InventoyUI inventoyUI;
    public EquipmetInventoryUI equipmetInventoryUI;
    public StatusUI statusUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        inventoyUI = GetComponent<InventoyUI>();
        equipmetInventoryUI = GetComponent<EquipmetInventoryUI>();
        statusUI = GetComponent<StatusUI>();

        StatusBtn.onClick.AddListener(() => OnStatus());
        InvenBtn.onClick.AddListener(() => OnInven());
        EquipBtn.onClick.AddListener(() => OnEquip());
        StartButton.onClick.AddListener(() => OnChangeCombatScene());

        OnStatus();
        OnInven();
        OnEquip();
    }

	public void OnChangeCombatScene()
    {
        SceneManager.LoadScene("InGame");
        PlayerManager.instance.cameraContorller.offset = new Vector3(-0.59f, -0.56f, 0.68f);
    }

    public void OnStatus()
    {
        statusUI.AllUpdateUI();
        statusUI.OnExitButton();
    }
    public void OnInven()
    {
        inventoyUI.OnOff();
    }
    public void OnEquip()
    {
        equipmetInventoryUI.OnOff();
    }
}
