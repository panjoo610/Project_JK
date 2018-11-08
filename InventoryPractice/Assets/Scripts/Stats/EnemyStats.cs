using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {
    EnemyAnimator enemyAnimator;
    Enemy enemy;

    public void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemy = GetComponent<Enemy>();
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
        PlayerManager.instance.playerController.RemoveFocus();
        enemy.enabled = false;
        //add ragdooll effect death animation

        Invoke("PushToPool", 1.3f);       
    }

    void PushToPool()
    {
        EnemyManager.instance.enemyPool.Push(gameObject);
    }


    private void OnEnable()
    {
        if(enemy != null)
        {
            enemy.enabled = true;
        }      
        currentHealth = maxHealth;
    }
}
