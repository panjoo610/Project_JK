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
    }
    #endregion

    public SaveInventory saveInventory;

    public GameObject Player, mainCamera;

    public PlayerStats playerStats;
    public PlayerController playerController;

    public TextMeshProUGUI PlayerGoldText, PlayerDamageText, PlayerArmorText;

    [SerializeField]
    public CameraContorller cameraContorller;

    public void Start()
    {
        Initialization();

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

    public void Initialization()
    {
        playerStats = Player.GetComponent<PlayerStats>();
        playerController = Player.GetComponent<PlayerController>();
       
        cameraContorller = mainCamera.GetComponent<CameraContorller>();
    }

    public void ResetPlayerPosition()
    {
        Player.transform.position = new Vector3(0.0f, 10.8f, -15.49f);
        cameraContorller.offset = new Vector3(-0.17f, -0.2f, -0.12f);
        playerStats.Initialization();
        Player.transform.rotation = Quaternion.identity;
    }

    public void UpdateStatusUI()
    {
        DamageCounter(playerStats.damage.GetValue());
        ArmorCounter(playerStats.armor.GetValue());
    }

    public void ShowPlayerGold(int gold)
    {
        if (gold > 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", saveInventory.PlayerGold, "to", gold, "onUpdate", "GoldCounter", "delay", 0, "time", 2));

            saveInventory.PlayerGold = gold;
            saveInventory.SaveItemListByJson();
        }
        else
        {
            return;
        }

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
