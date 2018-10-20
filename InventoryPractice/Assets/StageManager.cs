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
        PlayerManager.instance.cameraContorller.offset = new Vector3(-0.17f, -0.2f, -0.12f);
        PlayerManager.instance.cameraContorller.currentZoom = 15f;
    }

    public void ChangeCombatStage()
    {
        NoticeText.text = name + " Stage - " + CurrentStage.ToString();
        EnemyManager.instance.GenerateEnemy(CurrentStage);
        SceneManager.LoadScene(StageName.Stage + CurrentStage.ToString());
        PlayerManager.instance.cameraContorller.offset = new Vector3(-1f, -1.5f, 0f);
        PlayerManager.instance.cameraContorller.currentZoom = 10f;
    }

    public void ClearStage()
    {
        StartCoroutine(ShowClearText());

        if (OnGameClearCallBack != null)
            OnGameClearCallBack.Invoke();

        CurrentStage += 1;
        //게임을 완전히 클리어했다면 걸린 시간과 비교해서 골드를 줄 것과 획득 결과창만들 것
        
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
