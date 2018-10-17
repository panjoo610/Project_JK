using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {
    EnemyAnimator enemyAnimator;

    public void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemyAnimator.GetHitEffect();
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.instance.saveInventory.PlayerGold += 100;
        //add ragdooll effect death animation
        Invoke("PushToPool", 2f);
        
    }

    void PushToPool()
    {
        EnemyManager.instance.enemyPool.Push(gameObject);
    }
}
