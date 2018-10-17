using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    #region Singleton
    public static StageManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public SaveInventory saveInventory;

    public TextMeshProUGUI NoticeText, ClearText;
    public Button LobbyButton;

    public int CurrentStage = 1;

    // Use this for initialization
    void Start()
    {
        NoticeText.text = GetCurrentSceneName();
        LobbyButton.onClick.AddListener(() => MoveLobbyScene());
        if (saveInventory.CurrentStage == 0)
        {
            saveInventory.CurrentStage = CurrentStage;
            saveInventory.SaveItemListByJson();
        }
    }
    public void MoveLobbyScene()
    {
        NoticeText.text = StageName.Lobby.ToString();
        LobbyButton.gameObject.SetActive(false);
        SceneManager.LoadScene(StageName.Lobby.ToString());
    }

    public void ChangeCombatStage()
    {
        NoticeText.text = name + " Stage - " + CurrentStage.ToString();
        EnemyManager.instance.GenerateEnemy(CurrentStage);
        SceneManager.LoadScene(StageName.InGame + CurrentStage.ToString());
    }

    public void ClearStage()
    {
        StartCoroutine(ShowClearText());

        CurrentStage += 1;
        saveInventory.CurrentStage = CurrentStage;
        saveInventory.SaveItemListByJson();

        ShowLobbyBtn();
    }

    IEnumerator ShowClearText()
    {
        ClearText.gameObject.SetActive(true);
        ClearText.text = "Stage" + CurrentStage.ToString() + " Clear !";
        yield return new WaitForSeconds(1.0f);
        ClearText.gameObject.SetActive(false);
    }

    void ShowLobbyBtn()
    {
        LobbyButton.gameObject.SetActive(true);
    }

    public string GetCurrentSceneName()
    {
        string temp;
        temp = SceneManager.GetActiveScene().name;
        return temp;
    }
}
public enum StageName { Lobby, InGame, Stage}
