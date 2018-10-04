using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "Data/Status")]
public class PlayerStatusData : ScriptableObject
{
    public string StatusName = "New Status";
    public string StatusExplain = "Test Status";
    public Sprite icon = null;
    public int Damege = 0;
    public int Armor = 0;
    public int Price = 0;

    public KindOfStatus kindOfStatus;

    public void AddStatus(KindOfStatus kindOfStatus)
    {
        int DamageValue = 0;
        int ArmorValue = 0;

        switch (kindOfStatus)
        {
            case KindOfStatus.Damage:
                if (Damege > 0)
                    DamageValue = Damege * GetPlayerStatusCount(KindOfStatus.Damage);

                    PlayerManager.instance.ShowPlayerStatsDamage(PlayerManager.instance.playerStats.damage.GetValue() + DamageValue);

                    PlayerManager.instance.playerStats.damage.AddModifier(DamageValue);

                    PlayerManager.instance.saveInventory.DamageModifiers.Add(DamageValue);
                break;
            case KindOfStatus.Armor:
                if (Armor > 0)
                    ArmorValue = Armor * GetPlayerStatusCount(KindOfStatus.Armor);

                    PlayerManager.instance.ShowPlayerStatsArmor(PlayerManager.instance.playerStats.armor.GetValue() + ArmorValue);

                    PlayerManager.instance.playerStats.armor.AddModifier(ArmorValue);

                    PlayerManager.instance.saveInventory.AromorModifiers.Add(ArmorValue);
                break;
            default:
                break;
        }
    }
    public int GetPlayerStatusCount(KindOfStatus kindOfStatus)
    {
        int returnValue = 0;
        switch (kindOfStatus)
        {
            case KindOfStatus.Damage:
                if (Damege > 0)
                    returnValue =  PlayerManager.instance.saveInventory.DamageModifiers.Count;
                break;
            case KindOfStatus.Armor:
                if (Armor > 0)
                    returnValue = PlayerManager.instance.saveInventory.AromorModifiers.Count;
                break;
            default:
                break;
        }
        return returnValue + 1;
    }
    public int GetPrice(KindOfStatus kindOfStatus)
    {
        int returnValue = 0;
        switch (kindOfStatus)
        {
            case KindOfStatus.Damage:
                if (Price > 0)
                    returnValue = Price * GetPlayerStatusCount(KindOfStatus.Damage);
                if(GetPlayerStatusCount(KindOfStatus.Damage) == 0)
                {
                    returnValue = Price;
                }
                break;
            case KindOfStatus.Armor:
                if (Price > 0)
                    returnValue = Price * GetPlayerStatusCount(KindOfStatus.Armor);
                if (GetPlayerStatusCount(KindOfStatus.Armor) == 0)
                {
                    returnValue = Price;
                }
                break;
            default:
                break;
        }
        return returnValue;
    }
    
}
public enum KindOfStatus { Damage, Armor}
