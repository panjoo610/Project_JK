using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    public Image GunImgae;

    void Start ()
    {
        //equimentManager = EquimentManager.instance;
        //equimentManager.onEquipmentChanged += OnEquipmentChanged;

        ChangeGunImage();

    }

    //void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    //{
    //    ChangeGunImage();
    //}

    void ChangeGunImage()
    {
        GunImgae.sprite = EquimentManager.instance.currentEquiment[3].icon;
    }

}
