using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator {

    public WeaponAnimations[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationsDic;

    protected override void Start()
    {
        base.Start();
        EquimentManager.instance.onEquipmentChanged += OnEquipmentChanged;

        weaponAnimationsDic = new Dictionary<Equipment, AnimationClip[]>();
        foreach(WeaponAnimations a in weaponAnimations)
        {
            weaponAnimationsDic.Add(a.weapon, a.clips);
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
           // animator.SetLayerWeight(1, 1);
            if (weaponAnimationsDic.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationsDic[newItem];
            }
        }
        //else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon)
        //{
        //    animator.SetLayerWeight(1, 0);
        //    currentAttackAnimSet = defaultAttackAnimSet;
        //}
        //if (newItem != null && newItem.equipSlot == EquipmentSlot.Head)
        //{
        //    animator.SetLayerWeight(2, 1);
        //}
        //else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Head)
        //{
        //    animator.SetLayerWeight(2, 0);
        //}
    }

    [System.Serializable]
    public struct WeaponAnimations
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
 