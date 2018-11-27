using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIController : MonoBehaviour
{

    public Button StatusBtn, InvenBtn, EquipBtn, combatStartButton, LobbyButton, GameStartButton, StopButton;
    public GameObject InformationPanel, CombatPanel, HidePanel, NoticePanel, GameTitle, ExitPanel;
    public InventoyUI inventoyUI;
    public EquipmetInventoryUI equipmetInventoryUI;
    public StatusUI statusUI;
    public CombatUI combatUI;
    JoySickInputPanel joySickInputPanel;


    private void Awake()
    {
        GameTitle.SetActive(true);
        CombatPanel.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        StageManager.instance.OnGameClearCallBack += ShowLobbyBtn;
        StageManager.instance.OnMoveLobbySceneCallBack += MoveLobbyWhenGameOver;
        StageManager.instance.OnGameOverCallBack += ShowHidePaenl;
        StageManager.instance.OnClickAndroidBackButtonEvent += OnExitPanel;

        inventoyUI = GetComponent<InventoyUI>();
        equipmetInventoryUI = GetComponent<EquipmetInventoryUI>();
        statusUI = GetComponent<StatusUI>();
        combatUI = CombatPanel.GetComponent<CombatUI>();
        joySickInputPanel = GetComponent<JoySickInputPanel>();

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
        if (StageManager.instance.CurrentStage >= 5)
        {
            StageManager.instance.ChangeCombatStage();
            return;
        }
        else
        {
            statusUI.statusUI.gameObject.SetActive(false);
            inventoyUI.inventotyUI.gameObject.SetActive(false);
            equipmetInventoryUI.inventotyUI.gameObject.SetActive(false);

            StageManager.instance.ChangeCombatStage();
            ChangeCombatOrLobbyUI();
        }
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

        CombatPanel.SetActive(!CombatPanel.activeSelf);

        combatUI.ChangeGunImage();

        StartCoroutine(ShowHidePanelCoroutine(3.0f));
    }

    IEnumerator ShowHidePanelCoroutine(float time)
    {
        if (HidePanel.activeSelf)
        {
            HidePanel.SetActive(false);
            yield return null;
        }
        else
        {
            HidePanel.SetActive(true);
            yield return new WaitForSeconds(time);
            HidePanel.SetActive(false);
        }
    }

    public void ShowHidePaenl()
    {
        EnemyManager.instance.StageExit();
        NoticePanel.SetActive(false);
        StopButton.interactable = false;
        HidePanel.SetActive(true);
    }

    public void MoveLobbyWhenGameOver()
    {
        ExitPanel.SetActive(false);
        StopButton.interactable = false;
        NoticePanel.SetActive(false);
        HidePanel.SetActive(false);
        ChangeCombatOrLobbyUI();

        PlayerManager.instance.ShowPlayerGold(-100);

        StageManager.instance.MoveLobbyScene();
    }


    public void OnclickFirstStart()
    {
        GameStartButton.gameObject.SetActive(false);
        StageManager.instance.FirstLobby();
    }

    public void OnClickStopPanel()
    {
        HidePanel.SetActive(!HidePanel.activeInHierarchy);
        NoticePanel.SetActive(!NoticePanel.activeSelf);
    }


    public void OnclickStopCombatStage()
    {
        PlayerManager.instance.playerController.RemoveFocus();
        EnemyManager.instance.StageExit();

        HidePanel.SetActive(false);
        ChangeCombatOrLobbyUI();
        OnClickStopPanel();

        PlayerManager.instance.ShowPlayerGold(-100);
        joySickInputPanel.isGameState = false;


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
        HidePanel.SetActive(true);
        NoticePanel.SetActive(false);
        StopButton.interactable = false;
        LobbyButton.gameObject.SetActive(true);

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

    public void OnExitPanel()
    {
        ExitPanel.SetActive(!ExitPanel.activeSelf);
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
}
