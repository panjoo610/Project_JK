using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour {

    public CharacterStats myStats;

    public float attackSpeed = 2f;
    public float attackCoolDown = 0f;
    public float lastAttackTime;

    public const float combatCoolDown = 2.0f;

    public float attackDelay = 0.2f;

    public event System.Action OnAttack;

    public bool InCombat, Die;

    public CharacterStats oppoenentStats;

    public virtual void Start()
    {
        Die = false;
        myStats = GetComponent<CharacterStats>();
        if(myStats == null)
        {
            myStats = GetComponent<EnemyStats>();
        }
    }

    public virtual void Update()
    {
        attackCoolDown -= Time.deltaTime;
        
        if(Time.time - lastAttackTime > combatCoolDown)
        {
            InCombat = false;
        }
    }

    public virtual void Attack(CharacterStats targetStats)
    {
        if(attackCoolDown <= 0f)
        {
            oppoenentStats = targetStats;

            if (OnAttack != null)
            {
                OnAttack();
                if (targetStats.tag=="Player")
                {
                    Invoke("AttackHit_AnimationEvent", 0.5f);
                    SoundManager.instance.PlaySFX("MonsterAttack", false);
                }
            }
            attackCoolDown = 0.5f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        }
    }


    public virtual void AttackHit_AnimationEvent() 
    {
        oppoenentStats.TakeDamage(myStats.damage.GetValue());
        
        if (oppoenentStats.currentHealth <= 0)
        {
            InCombat = false;        
        }
    }
}
