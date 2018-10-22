using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{
    Idle, Move, Attack, Skill, Dead
}

public class BossController : MonoBehaviour {
    

    BossAnimator animator;
    BossMovement movement;
    BossState state;

    public Transform target;
    public float lookRadius;
    NavMeshAgent agent;
    bool IsEngage;
    float coolTime;

    void Start () {
        Initialize();
	}

    void Initialize()
    {
        //target = PlayerManager.instance.transform;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<BossAnimator>();
        ChangeState(BossState.Idle);
    }

    void Update () {
        if (!IsEngage) //플레이어를 발견조건
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                ChangeState(BossState.Move);
                IsEngage = true;
            }
        }
        else //교전시작후 스킬 쿨타임
        {
            float runtime = Time.deltaTime;
            if (runtime >= coolTime)
            {
                ChangeState(BossState.Skill);
                runtime = 0;
            }
        }
        
        movement.Behaviour();
        Debug.Log(state + " 0");
        if (movement.CheckIsDone(out state))
        {
            Debug.Log(state + " 1");
            movement.CheckIsDone(out state);
            Debug.Log(state + " 2");
            AnimationState(state);
        }
	}

    private void AnimationState(BossState state)
    {
        switch (state)
        {
            case BossState.Attack:
                animator.Attack();
                //EnemyCombet.Attack();
                Debug.Log("Attack");
                break;
            case BossState.Skill:
                Debug.Log("Skill");
                //EnemyCombet.Attack();
                animator.Skill();
                break;
            default:
                break;
        }
    }
    private void ChangeState(BossState state)
    {
        switch (state)
        {
            case BossState.Idle:
                movement = new IdleMovement(agent, transform, 1f);
                animator.Idle();
                break;
            case BossState.Move:
                movement = new NomalMovement(agent, target, transform, 1f);
                break;
            case BossState.Skill:
                movement = new StraightMovement(agent, target, transform, 3f);
                break;
            case BossState.Dead:
                movement = new DoNotMovement();
                animator.Dead();
                break;
            default:
                break;
        }
    }
}

