using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public override void Attack(CharacterStats targetStats)
    {
        base.Attack(targetStats);

        AttackHit_AnimationEvent();
    }
}
