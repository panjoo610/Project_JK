using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIController : MonoBehaviour {

    public Button StatusBtn, InvenBtn, EquipBtn, GameStartButton;
    public GameObject InformationPanel, CombatPanel, HidePanel;
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
        GameStartButton.onClick.AddListener(() => OnChangeCombatScene());

        OnStatus();
        OnInven();
        OnEquip();
    }

	public void OnChangeCombatScene()
    {
        ChangeCombatOrLobbyUI();

        statusUI.statusUI.gameObject.SetActive(false);
        inventoyUI.inventotyUI.gameObject.SetActive(false);
        equipmetInventoryUI.inventotyUI.gameObject.SetActive(false);

        StageManager.instance.ChangeCombatStage(StageName.InGame.ToString());
        PlayerManager.instance.cameraContorller.offset = new Vector3(-0.59f, -0.56f, 0.68f);         
    }

    public void ChangeCombatOrLobbyUI()
    {
        StatusBtn.gameObject.SetActive(!StatusBtn.gameObject.activeSelf);
        InvenBtn.gameObject.SetActive(!InvenBtn.gameObject.activeSelf);
        EquipBtn.gameObject.SetActive(!EquipBtn.gameObject.activeSelf);
        GameStartButton.gameObject.SetActive(!GameStartButton.gameObject.activeSelf);
        InformationPanel.SetActive(!InformationPanel.activeSelf);

        HidePanel.SetActive(!HidePanel.activeSelf);
        CombatPanel.SetActive(!CombatPanel.activeSelf);
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
