using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    public Image GunImgae;
    InventoryManager inventory;
    Text currntCount;
    Text goalCount;

    private void Start()
    {
        currntCount = transform.GetChild(5).GetComponent<Text>();
        currntCount.text = "남은 제거 대상 : " + EnemyManager.instance.GenerateDatas[EnemyManager.instance.currentStage].currentCount.ToString();
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
        
    }

}
