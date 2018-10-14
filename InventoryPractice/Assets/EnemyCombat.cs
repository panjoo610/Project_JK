using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{

    public override void Attack(CharacterStats targetStats)
    {
        if (myStats.currentHealth>=1)
        {
            Debug.Log("base.attack");
            base.Attack(targetStats);
           
        }
    }
    //PlayerManager.instance.cameraContorller.ShakeCamera(); //플레이어매니저로 이동할 것
}
