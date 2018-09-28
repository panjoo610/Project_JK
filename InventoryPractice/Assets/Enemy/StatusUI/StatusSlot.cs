using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusSlot : MonoBehaviour {

    public Text NameText,ExplainText, PriceText, levelText;
    public Image Icon;
    public int Price;
    //public Button BuyButton;

    public PlayerStatusData playerStatusData;

    private void Start()
    {
        Price = playerStatusData.GetPrice(playerStatusData.kindOfStatus);
        UpdateUI();
    }

    public void UpdateUI()
    {
        Price = playerStatusData.GetPrice(playerStatusData.kindOfStatus);

        levelText.text = "Level : " + playerStatusData.GetPlayerStatusCount(playerStatusData.kindOfStatus).ToString();

        NameText.text = playerStatusData.StatusName;

        ExplainText.text = playerStatusData.StatusExplain;

        PriceText.text = "Price : " + Price.ToString();

        Icon.sprite = playerStatusData.icon;    
    }

    public void OnClickStatusUp()
    {
        if (PlayerManager.instance.saveInventory.PlayerGold < Price)
        {
            Debug.Log("구매불가");
        }
        else
        {
            playerStatusData.AddStatus(playerStatusData.kindOfStatus);


            int temp = PlayerManager.instance.saveInventory.PlayerGold - Price;

            PlayerManager.instance.saveInventory.SaveItemListByJson();
            PlayerManager.instance.ShowPlayerGold(temp);           
        }
        UpdateUI();
    }
}
