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
        switch (kindOfStatus)
        {
            case KindOfStatus.Damage:
                if(Damege > 0)
                    PlayerManager.instance.playerStats.damage.AddModifier(Damege * GetPlayerStatusCount(KindOfStatus.Damage));
                    PlayerManager.instance.saveInventory.DamageModifiers.Add(Damege * GetPlayerStatusCount(KindOfStatus.Damage));
                break;
            case KindOfStatus.Armor:
                if (Armor > 0)
                    PlayerManager.instance.playerStats.armor.AddModifier(Armor * GetPlayerStatusCount(KindOfStatus.Armor));
                    PlayerManager.instance.saveInventory.AromorModifiers.Add(Armor * GetPlayerStatusCount(KindOfStatus.Armor));
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
