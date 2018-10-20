using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{

    public override void Attack(CharacterStats targetStats)
    {
        if (myStats.currentHealth>=1)
        {
            base.Attack(targetStats);
        }
    }
}
