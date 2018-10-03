using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterAnimator : MonoBehaviour {

    public AnimationClip replaceableAttackAnim;
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;
    

    const float locomationAnimationSmoothTime = 0.1f;
    protected NavMeshAgent agent;

    protected Animator animator;

    protected CharacterCombat combat;
    protected AnimatorOverrideController overrideController;


	// Use this for initialization
	protected virtual void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();
        if(overrideController == null)
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;

        animator.SetFloat("SpeedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);

        animator.SetBool("InCombat", combat.InCombat);
	}

     protected virtual void OnAttack()
     {
        animator.SetTrigger("Attack");
        int attackIndex = Random.Range(0, currentAttackAnimSet.Length);
        overrideController[replaceableAttackAnim.name] = currentAttackAnimSet[attackIndex];
     }
}
