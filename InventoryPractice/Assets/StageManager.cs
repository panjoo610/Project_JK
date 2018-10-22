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

    public string nextScene;

    [SerializeField]
    Image progressBar;
    [SerializeField]
    GameObject loadingPanel;

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

    public void FirstLobby()
    {
        LoadScene(StageName.Lobby.ToString());

        PlayerManager.instance.ResetPlayerPosition();
    }

    public void MoveLobbyScene()
    {
        NoticeText.text = StageName.Lobby.ToString();

        EquimentManager.instance.saveInventory.SaveItemListByJson();

        LoadScene(StageName.Lobby.ToString());
        
        PlayerManager.instance.ResetPlayerPosition();
    }

    public void ChangeCombatStage()
    {
        NoticeText.text = name + " Stage - " + CurrentStage.ToString();
        EnemyManager.instance.GenerateEnemy(CurrentStage);
        LoadScene(StageName.Stage + CurrentStage.ToString());
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

    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        loadingPanel.SetActive(!loadingPanel.activeSelf);
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }            
        }
        loadingPanel.SetActive(!loadingPanel.activeSelf);
        progressBar.fillAmount = 0;
    }
}
public enum StageName { Lobby, InGame, Stage}
