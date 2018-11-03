using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombet : CharacterCombat {
    
    public override void Attack(CharacterStats targetStats)
    {
        if (myStats.currentHealth >= 1)
        {
            oppoenentStats = targetStats;
            AttackHit_AnimationEvent();
        }
    }
}
