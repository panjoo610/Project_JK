using System.Collections;
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
        DontDestroyOnLoad(gameObject);
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
        cameraContorller = Camera.main.GetComponent<CameraContorller>();

        for (int i = 0; i < saveInventory.DamageModifiers.Count; i++)
        {
            playerStats.damage.modifiers.Add(saveInventory.DamageModifiers[i]);
        }
        for (int i = 0; i < saveInventory.AromorModifiers.Count; i++)
        {
            playerStats.armor.modifiers.Add(saveInventory.AromorModifiers[i]);
        }


        //플레이어가 가진 점수나 골드를 이 함수로 표현할 수 있도로 함
        GoldCounter(saveInventory.PlayerGold);
    }

    public void UpdateStatusUI()
    {
        DamageCounter(playerStats.damage.GetValue());
        ArmorCounter(playerStats.armor.GetValue());
    }

    public void ShowPlayerGold(int gold)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", saveInventory.PlayerGold, "to", gold, "onUpdate", "GoldCounter", "delay", 0, "time", 2));

        saveInventory.PlayerGold = gold;
        saveInventory.SaveItemListByJson();
    }

    public void ShowPlayerStatsDamage(int Damage)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", playerStats.damage.GetValue(), "to", Damage, "onUpdate", "DamageCounter", "delay", 0, "time", 1));
    }
    public void ShowPlayerStatsArmor(int Armor)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", playerStats.armor.GetValue(), "to", Armor, "onUpdate", "ArmorCounter", "delay", 0, "time", 1));
    }


    public void GoldCounter(int statsNum)
    {
        PlayerGoldText.text = statsNum.ToString();
    }
    public void DamageCounter(int DamageValue)
    {
        PlayerDamageText.text = DamageValue.ToString();
    }
    public void ArmorCounter(int ArmorValue)
    {
        PlayerArmorText.text = ArmorValue.ToString();
    }

}
