using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {


	
	void Start ()
    {     
        EquimentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
        }

        if(oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
        }
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        SoundManager.instance.PlaySFX("PlayerHit", false);
        PlayerManager.instance.cameraContorller.ShakeCamera(); //플레이어매니저로 이동할 것
    }

    public override void Die()
    {
        base.Die();
        //KILL THE PLAYER
        //게임오버스크린 , 패널티 , 리스폰
        //리스타트 씬
        //PlayerManager.instance.KillPlayer();
    }
}
