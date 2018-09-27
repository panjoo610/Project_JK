using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour {

    #region
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public SaveInventory saveInventory;

    public GameObject Player;

    public PlayerStats playerStats;

    public Text TestGoldText;


    public void Start()
    {
        playerStats = Player.GetComponent<PlayerStats>();

        //playerStats.damage.AddModifier(30);
        

        //플레이어가 가진 점수나 골드를 이 함수로 표현할 수 있도로 함
        Counter(saveInventory.PlayerGold);
    }
    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowPlayerGold(int gold)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", saveInventory.PlayerGold, "to", gold, "onUpdate", "Counter", "delay", 0, "time", 2));

        saveInventory.PlayerGold = gold;
        saveInventory.SaveItemListByJson();
    }

    void Counter(int statsNum)
    {
        TestGoldText.text = statsNum.ToString();
    }
}
