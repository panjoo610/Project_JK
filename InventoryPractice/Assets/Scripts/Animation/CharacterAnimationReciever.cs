using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationReciever : MonoBehaviour {

    public CharacterCombat combat;

    public void AttackHitEvent()
    {
        combat.AttackHit_AnimationEvent();//공격 시작시 애니메이션 이벤트 시작
    }
}
