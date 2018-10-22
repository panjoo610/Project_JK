using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAnimator : MonoBehaviour {
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Animator animator;
    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
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
        animator.SetTrigger("Dead");
    }
    public void Shout()
    {

    }
}
