using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public SkinnedMeshRenderer mesh;

    public EquipmentSlot equipSlot;

    public int armorModifier;
    public int damageModifier;

    public Sprite combatImage = null;

    public override void Use()
    {
        base.Use();

        EquimentManager.instance.Equip(this);

        PlayerManager.instance.DamageCounter(PlayerManager.instance.playerStats.damage.GetValue());
        PlayerManager.instance.ArmorCounter(PlayerManager.instance.playerStats.armor.GetValue());
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Arms}