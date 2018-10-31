using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    public Image GunImgae;
    InventoryManager inventory;


    public void ChangeGunImage()
    {
        if(EquimentManager.instance.currentEquiment[3].combatImage != null)
        {
            GunImgae.sprite = EquimentManager.instance.currentEquiment[3].combatImage;
        }
        else
        {
            return;
        }   
    }

}
