﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI PlayerGoldText, PlayerDamageText, PlayerArmorText;

    [SerializeField]
    public CameraContorller cameraContorller;

    public void Start()
    {
        playerStats = Player.GetComponent<PlayerStats>();

        for (int i = 0; i < saveInventory.DamageModifiers.Count; i++)
        {
            playerStats.damage.modifiers.Add(saveInventory.DamageModifiers[i]);
        }
        for (int i = 0; i < saveInventory.AromorModifiers.Count; i++)
        {
            playerStats.armor.modifiers.Add(saveInventory.AromorModifiers[i]);
        }
        cameraContorller = Camera.main.GetComponent<CameraContorller>();

        //플레이어가 가진 점수나 골드를 이 함수로 표현할 수 있도로 함
        GoldCounter(saveInventory.PlayerGold);

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

    public void ShowPlayerStats(int Damage, int Armor)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", playerStats.armor.GetValue(), "to", Damage, "onUpdate", "Counter", "delay", 0, "time", 2));
        iTween.ValueTo(gameObject, iTween.Hash("from", playerStats.armor.GetValue(), "to", Armor, "onUpdate", "Counter", "delay", 0, "time", 2));
        //카운터를 대미지카운터, 아머카운터로 바꾸고 호출을 up할때 할 것! 
        //saveInventory.PlayerGold = gold;

        saveInventory.SaveItemListByJson();
    }


    void GoldCounter(int statsNum)
    {
        PlayerGoldText.text = statsNum.ToString();
    }
}
