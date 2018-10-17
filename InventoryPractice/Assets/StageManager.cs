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
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public SaveInventory saveInventory;

    public TextMeshProUGUI NoticeText, ClearText;

    public int CurrentStage = 1;

    public delegate void OnGameClear();
    public OnGameClear OnGameClearCallBack;

    // Use this for initialization
    void Start() //초기화 함수 Initialization
    {
        NoticeText.text = GetCurrentSceneName();

        if (saveInventory.CurrentStage == 0)
        {
            saveInventory.CurrentStage = CurrentStage;
            saveInventory.SaveItemListByJson();
        }
        else
        {
            CurrentStage = saveInventory.CurrentStage;
        }
    }
    public void MoveLobbyScene()
    {
        NoticeText.text = StageName.Lobby.ToString();

        EquimentManager.instance.saveInventory.SaveItemListByJson();

        SceneManager.LoadScene(StageName.Lobby.ToString());
        
        PlayerManager.instance.ResetPlayerPosition();
    }

    public void ChangeCombatStage()
    {
        NoticeText.text = name + " Stage - " + CurrentStage.ToString();
        EnemyManager.instance.GenerateEnemy(CurrentStage);
        SceneManager.LoadScene(StageName.Stage + CurrentStage.ToString());
    }

    public void ClearStage()
    {
        StartCoroutine(ShowClearText());

        if (OnGameClearCallBack != null)
            OnGameClearCallBack.Invoke();
   
        CurrentStage += 1;
        saveInventory.CurrentStage = CurrentStage;
        saveInventory.SaveItemListByJson();
    }

    IEnumerator ShowClearText()
    {
        ClearText.gameObject.SetActive(true);
        ClearText.text = "Stage" + CurrentStage.ToString() + " Clear !";
        yield return new WaitForSeconds(3.0f);
        ClearText.gameObject.SetActive(false);
    }

    public string GetCurrentSceneName()
    {
        string temp;
        temp = SceneManager.GetActiveScene().name;
        return temp;
    }
}
public enum StageName { Lobby, InGame, Stage}
