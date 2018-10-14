using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public TextMeshProUGUI NoticeText;

    public int CurrentStage = 1;

    // Use this for initialization
    void Start ()
    {
        NoticeText.text = GetCurrentSceneName();

        if(saveInventory.CurrentStage == 0)
        {
            saveInventory.CurrentStage = CurrentStage;
            saveInventory.SaveItemListByJson();
        }
    }
    public void MoveLobbyScene()
    {
        NoticeText.text = StageName.Lobby.ToString();
        SceneManager.LoadScene(StageName.Lobby.ToString());
    }

    public void ChangeCombatStage(string name)
    {
        NoticeText.text = name +" Stage - " + saveInventory.CurrentStage.ToString();
        //EnemyManager.instance.GenerateEnemy(currentStage);
        SceneManager.LoadScene(name);
    }

    public void ClearStage()
    {
        //EnemyManager 쪽에서 클리어 할 시에 명시적으로 부름.
        CurrentStage += 1;
        saveInventory.CurrentStage = CurrentStage;
        saveInventory.SaveItemListByJson();
    }

    public string GetCurrentSceneName()
    {
        string temp;
        temp = SceneManager.GetActiveScene().name;
        return temp;
    }
}
public enum StageName { Lobby, InGame, Stage1, Stage2, Stage3,}
