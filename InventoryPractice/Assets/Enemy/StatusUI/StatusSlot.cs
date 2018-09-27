using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusSlot : MonoBehaviour {

    public Text NameText,ExplainText;
    public Image Icon;
    public int Price;
    //public Button BuyButton;

    public PlayerStatusData playerStatusData;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {

        NameText.text = playerStatusData.StatusName;
        ExplainText.text = playerStatusData.StatusExplain;

        Icon.sprite = playerStatusData.icon;
        Price = playerStatusData.Price;
    }

    public void OnClickStatusUp()
    {
        if (PlayerManager.instance.saveInventory.PlayerGold < Price)
        {
            Debug.Log("구매불가");
        }
        else
        {
            PlayerManager.instance.playerStats.damage.AddModifier((int)playerStatusData.Damege);
            PlayerManager.instance.playerStats.armor.AddModifier((int)playerStatusData.Armor);


            if((int)playerStatusData.Damege > 0)
            {
                PlayerManager.instance.saveInventory.DamageModifiers.Add((int)playerStatusData.Damege);
            }
            if ((int)playerStatusData.Armor > 0)
            {
                PlayerManager.instance.saveInventory.AromorModifiers.Add((int)playerStatusData.Armor);
            }

            int temp = PlayerManager.instance.saveInventory.PlayerGold - Price;

            PlayerManager.instance.saveInventory.SaveItemListByJson();
            PlayerManager.instance.ShowPlayerGold(temp);
            UpdateUI();
        }
    }
}
