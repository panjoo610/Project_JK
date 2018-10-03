using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : CharacterAnimator {

    public int EnemyCount;
    public Animator[] animators;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
        animators = new Animator[EnemyCount];
        animators = GetComponentsInChildren<Animator>();

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;
    }
    protected override void Update()
    {
        //base.Update();
    }
    protected override void OnAttack()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("attack1"); 
        }
    }

}
