using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossMovementState
{
    Stop, Idle, Move, Skill
}
public enum BossActionState
{
    Attack, Skill, Dead, Shout
}

public class BossController : MonoBehaviour {
    

    BossAnimator animator;
    BossMovement movement;
    BossMovementState MovementState;

    public Transform target;
    public float lookRadius;
    NavMeshAgent agent;
    bool IsEngage;
    bool IsSkillActive;
    bool IsAttacking;
    public float coolTime;
    //public float NomalAttackDistance;
    //public float SkillAttackDistance;
    //public float StoppingDistance;
    float runtime;

    void Start () {
        Initialize();
	}

    void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        //target = PlayerManager.instance.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<BossAnimator>();
        ChangeMovementState(BossMovementState.Idle);
    }

    void Update () {
        if (!IsEngage) //플레이어를 발견조건
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                ChangeMovementState(BossMovementState.Move);
                IsEngage = true;
            }
        }
        else //교전시작후 스킬 쿨타임
        {
            runtime += Time.deltaTime;
            if (runtime >= coolTime && IsSkillActive == false && IsAttacking == false)
            {
                Debug.Log("스킬을 사용함 "+ runtime);
                StartCoroutine(UseSkill());
            }
        }
        
        movement.Behaviour();
        Debug.Log(MovementState + " 0");
        if (IsSkillActive == false && IsAttacking == false)
        {
            if (movement.CheckIsDone(out MovementState))
            {
                Debug.Log(MovementState);
                ActionByState(MovementState);
            } 
        }
	}

    private void ActionByState(BossMovementState state)
    {
        switch (state)
        {
            case BossMovementState.Idle:
                ChangeMovementState(state);
                break;
            case BossMovementState.Move:
                //animator.Attack();
                //ChangeMovementState(state);
                //EnemyCombet.Attack();
                if (IsAttacking == false && IsSkillActive == false)
                {
                    StartCoroutine(NomalAttack()); 
                }
                Debug.Log("Attack 애니메이션 재생");
                break;
            case BossMovementState.Skill:
                Debug.Log("Skill");
                //EnemyCombet.Attack();
                animator.Skill();
                ChangeMovementState(state);
                break;
            case BossMovementState.Stop:
                //ChangeMovementState(state);
                break;
            default:
                break;
        }
    }
    private void ChangeMovementState(BossMovementState state, bool IsLooking = false)
    {
        switch (state)
        {
            case BossMovementState.Idle:
                movement = new IdleMovement(agent, transform, 1f);
                break;
            case BossMovementState.Move:
                movement = new NomalMovement(agent, target, transform, 1f);
                break;
            case BossMovementState.Skill:
                movement = new StraightMovement(agent, target, transform, 5f);
                break;
            case BossMovementState.Stop:
                movement = new DoNotMovement(agent, IsLooking, transform,target);
                break;
            default:
                break;
        }
    }
    IEnumerator NomalAttack()
    {
        IsAttacking = true;
        animator.Attack();
        ChangeMovementState(BossMovementState.Stop,true);
        yield return new WaitForSeconds(3f);
        ChangeMovementState(BossMovementState.Move);
        IsAttacking = false;
    }

    IEnumerator UseSkill()
    {
        IsSkillActive = true;
        //(진입할때는 싸우는중이니 타겟을 바라본 상태다)
        //타겟을 바라보고,멈춘상태, 샤우팅 한번
        ChangeMovementState(BossMovementState.Stop);
        animator.Shout();
        yield return new WaitForSeconds(3f);

        //타겟이 있던 위치로 빠르게 전진
        ChangeMovementState(BossMovementState.Skill);
        
        //스킬공격 재생, 멈춘상태
        while (true)
        {
            yield return null;
            //movement.CheckIsDone(out state);
            Debug.Log(MovementState+"스킬공격 재생, 멈춘상태");
            Debug.Log(movement.CheckIsDone(out MovementState));
            if (movement.CheckIsDone(out MovementState))
            {
                ChangeMovementState(BossMovementState.Stop);
                animator.Skill();
                break;
            }
        }
        //샤우팅 한번,멈춘상태
        yield return new WaitForSeconds(1f);
        animator.Shout();
        yield return new WaitForSeconds(2f);


        ChangeMovementState(BossMovementState.Idle);
        IsEngage = false;
        IsSkillActive = false;
        runtime = 0;
        Debug.Log("스킬 끝");
        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

