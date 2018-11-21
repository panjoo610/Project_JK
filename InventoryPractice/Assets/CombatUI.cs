using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    public Image GunImgae;
    InventoryManager inventory;

    [SerializeField]
    Text currntCount;

    Text goalCount;

    private void Start()
    {
        currntCount.text = "남은 제거 대상 : " + EnemyManager.instance.EnemyCountInStage.ToString();
        EnemyManager.instance.OnChangeCountCallBack += UpdateEnemyCountUI;
    }

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
    public void UpdateEnemyCountUI()
    {
        currntCount.text = "남은 제거 대상 : " + EnemyManager.instance.EnemyCountInStage.ToString();
    }

}
