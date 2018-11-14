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

    public TextMeshProUGUI SceneNoticeText, NoticeText, ClearBonusText;

    public int CurrentStage = 1;

    public delegate void OnGameStart();
    public OnGameStart OnGameStartCallBack;

    public delegate void OnGameClear();
    public OnGameClear OnGameClearCallBack;

    public delegate void OnGameOver();
    public OnGameOver OnGameOverCallBack;

    public delegate void OnMoveLobbyScene();
    public OnMoveLobbyScene OnMoveLobbySceneCallBack;

    public string nextScene;

    const int LimitStageCount = 4;

    [SerializeField]
    Image progressBar;
    [SerializeField]
    GameObject loadingPanel;

    const int clearAmount = 1000;
    // Use this for initialization

    void Start() //초기화 함수 Initialization
    {
        NoticeText.gameObject.SetActive(false);
        SceneNoticeText.text = GetCurrentSceneName();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();
        }
    }

    public void FirstLobby()
    {
        LoadScene(StageName.Lobby.ToString());
        
        PlayerManager.instance.ResetPlayerPosition();
    }

    public void MoveLobbyScene()
    {
        ClearBonusText.gameObject.SetActive(false);
        NoticeText.gameObject.SetActive(false);

        PlayerManager.instance.ResetPlayerPosition();

        PlayerManager.instance.cameraContorller.HideHitImage();
        PlayerManager.instance.cameraContorller.RobbyCamera();
        LoadScene(StageName.Lobby.ToString());

        PlayerManager.instance.ResetPlayerPosition();
    }

    public void ChangeCombatStage()
    {
        if(CurrentStage > LimitStageCount)
        {
            return;
        }

        SceneNoticeText.text = " Stage - " + CurrentStage.ToString();
        EnemyManager.instance.GenerateEnemy(CurrentStage);

        LoadScene(StageName.Stage + CurrentStage.ToString());
        StartCoroutine(ShowGameStartText(2.0f));

        if (OnGameStartCallBack != null)
            OnGameStartCallBack.Invoke();

        PlayerManager.instance.cameraContorller.ActingCombat();
    }

    IEnumerator ShowGameStartText(float time)
    {
        NoticeText.gameObject.SetActive(true);
        NoticeText.text = "Game Start !";
        yield return new WaitForSeconds(time);
        NoticeText.gameObject.SetActive(false);

        PlayerManager.instance.ActivePlayerController(true);
    }

    public void ClearStage()
    {
        NoticeText.gameObject.SetActive(true);
        ClearBonusText.gameObject.SetActive(true);

        int bonousAmout = CurrentStage * clearAmount;
        PlayerManager.instance.ShowPlayerGold(bonousAmout);

        NoticeText.text = "Stage - " + CurrentStage.ToString() + " Clear !";
        ClearBonusText.text = "Clear Reward : $" + bonousAmout.ToString();

        if (OnGameClearCallBack != null)
            OnGameClearCallBack.Invoke();

        CurrentStage += 1;
        
        //게임을 완전히 클리어했다면 걸린 시간과 비교해서 골드를 줄 것과 획득 결과창만들 것
        
        saveInventory.CurrentStage = CurrentStage;
        saveInventory.SaveItemListByJson();   
    }


    public void GameOver()
    {
        NoticeText.gameObject.SetActive(true);
        NoticeText.text = "GAME OVER";

        AlreadyGameOver();

        Invoke("GameOverNotice", 3f);
    }
    public void GameOverNotice()
    {
        if (OnMoveLobbySceneCallBack != null)
            OnMoveLobbySceneCallBack.Invoke();
    }

    public void AlreadyGameOver()
    {
        if (OnGameOverCallBack != null)
            OnGameOverCallBack.Invoke();
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
        SceneNoticeText.text = GetCurrentSceneName();
    }
}
public enum StageName { Lobby, InGame, Stage}
