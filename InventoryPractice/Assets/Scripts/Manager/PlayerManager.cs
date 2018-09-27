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

    public GameObject Player;

    PlayerStats playerStats;

    public Text TestStatsText;

    public void Start()
    {
        playerStats = Player.GetComponent<PlayerStats>();

        //플레이어가 가진 점수나 골드를 이 함수로 표현할 수 있도로 함
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 100, "onUpdate", "Counter", "delay", 2, "time", 2));
    }
    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void Counter(int statsNum)
    {
        TestStatsText.text = statsNum.ToString();
    }
}
