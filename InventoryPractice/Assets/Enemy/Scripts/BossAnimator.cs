using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAnimator : EnemyAnimator
{
    //[SerializeField]
    //NavMeshAgent agent;
    //[SerializeField]
    //Animator animator;
    // Use this for initialization
    //public new ParticleSystem ParticleSystem;

    protected override void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        ParticleSystem.transform.position = transform.position;
    }
    protected override void Update()
    {
        float speedPercent = agent.velocity.magnitude / 2;// / agent.speed
        //Debug.Log(speedPercent);
        animator.SetFloat("Speed", speedPercent);
    }
    public void Attack()
    {
        animator.SetTrigger("Attack01");
    }
    public void Attack2()
    {
        animator.SetTrigger("Attack02");
    }
    public void Skill()
    {
        animator.SetTrigger("Attack03");
    }
    public void Idle()
    {

    }
    public void Dead()
    {
        if (isDie == false)
        {
            isDie = true;
            animator.SetTrigger("Dead");
        }
    }
    public void Shout()
    {
        animator.SetTrigger("Shout");
    }
    public override void GetHitEffect()
    {
        //animator.SetTrigger("GetHit");
        ParticleSystem.Play();
    }
}
