using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadStats : CharacterStats
{
    SquadTest squad;
    float Num;
    private void Start()
    {
        squad = GetComponent<SquadTest>();
        Num = squad.squadUnit.Count;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth<=maxHealth/Num)
        {
            Debug.Log(Num + ", " + maxHealth + ", " + currentHealth);
        }
    }

    public override void Die()
    {
        base.Die();

        //add ragdooll effect death animation
        Destroy(gameObject);
    }
}
