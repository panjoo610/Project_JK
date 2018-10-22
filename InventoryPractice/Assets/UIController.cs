﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIController : MonoBehaviour {

    public Button StatusBtn, InvenBtn, EquipBtn, combatStartButton, LobbyButton, GameStartButton, StopButton;
    public GameObject InformationPanel, CombatPanel, HidePanel, NoticePanel;
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
        StageManager.instance.OnGameClearCallBack += ShowLobbyBtn;
        StageManager.instance.OnGameOverCallBack += OnGameOver;

        inventoyUI = GetComponent<InventoyUI>();
        equipmetInventoryUI = GetComponent<EquipmetInventoryUI>();
        statusUI = GetComponent<StatusUI>();

        StatusBtn.onClick.AddListener(() => OnStatus());
        InvenBtn.onClick.AddListener(() => OnInven());
        EquipBtn.onClick.AddListener(() => OnEquip());
        combatStartButton.onClick.AddListener(() => OnChangeCombatScene());

        LobbyButton.onClick.AddListener(() => OnChangeLobbyScene());
        GameStartButton.onClick.AddListener(() => OnclickFirstStart());

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

        StageManager.instance.ChangeCombatStage();       
    }

    public void ChangeCombatOrLobbyUI()
    {
        StopButton.interactable = true;
        StatusBtn.gameObject.SetActive(!StatusBtn.gameObject.activeSelf);
        StopButton.gameObject.SetActive(!StopButton.gameObject.activeSelf);

        InvenBtn.gameObject.SetActive(!InvenBtn.gameObject.activeSelf);
        EquipBtn.gameObject.SetActive(!EquipBtn.gameObject.activeSelf);

        combatStartButton.gameObject.SetActive(!combatStartButton.gameObject.activeSelf);

        InformationPanel.SetActive(!InformationPanel.activeSelf);
        

        HidePanel.SetActive(!HidePanel.activeSelf);
        CombatPanel.SetActive(!CombatPanel.activeSelf);
    }

    public void OnclickFirstStart()
    {
        GameStartButton.gameObject.SetActive(false);
        StageManager.instance.FirstLobby();        
    }


    public void OnClickStopPanel()
    {
        NoticePanel.SetActive(!NoticePanel.activeSelf);
    }


    public void OnclickStopCombatStage()
    {
        PlayerManager.instance.playerController.RemoveFocus();
        EnemyManager.instance.StageExit();

        ChangeCombatOrLobbyUI();
        OnClickStopPanel();

        PlayerManager.instance.ShowPlayerGold(-100);

        StageManager.instance.MoveLobbyScene();
    }


    public void OnChangeLobbyScene()
    {
        ChangeCombatOrLobbyUI();

        StageManager.instance.MoveLobbyScene();

        LobbyButton.gameObject.SetActive(false);  
    }

    public void ShowLobbyBtn()
    {
        StopButton.interactable = false;
        LobbyButton.gameObject.SetActive(true);

        EnemyManager.instance.StageExit();
    }

    public void OnGameOver()
    {
        ChangeCombatOrLobbyUI();

        PlayerManager.instance.ShowPlayerGold(-100);

        StageManager.instance.MoveLobbyScene();

        EnemyManager.instance.StageExit();
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
