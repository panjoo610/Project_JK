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
    EnemyStats bossStats;
    BossCombet bossCombat;

    CharacterStats targetStats;
    [SerializeField]
    Collider[] attackCollider;

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

    IEnumerator attackCoroutine;
    IEnumerator skillCoroutine;

    void Start () {
        Initialize();
	}

    void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        //target = PlayerManager.instance.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetStats = target.GetComponent<CharacterStats>();
        animator = GetComponent<BossAnimator>();
        bossStats = GetComponent<EnemyStats>();
        bossCombat = GetComponent<BossCombet>();
        ChangeMovementState(BossMovementState.Idle);
        attackCollider = GetComponentsInChildren<Collider>();
        attackCoroutine = NomalAttack();
        skillCoroutine = UseSkill();
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
                skillCoroutine = UseSkill();
                StartCoroutine(skillCoroutine);
            }
        }
        
        movement.Behaviour();
        if (IsSkillActive == false && IsAttacking == false)
        {
            if (movement.CheckIsDone(out MovementState))
            {
                ActionByState(MovementState);
            } 
        }
	}
    private void LateUpdate()
    {
        if (bossStats.currentHealth <= 0)
        {
            ChangeMovementState(BossMovementState.Stop,false);
            animator.Dead();
            StopCoroutine(attackCoroutine);
            StopCoroutine(skillCoroutine);
            attackCoroutine = NomalAttack();
            skillCoroutine = UseSkill();
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
                if (IsAttacking == false && IsSkillActive == false)
                {
                    attackCoroutine = NomalAttack();
                    StartCoroutine(attackCoroutine); 
                }
                break;
            case BossMovementState.Skill:
                animator.Skill();
                ChangeMovementState(state);
                break;
            case BossMovementState.Stop:
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
        ChangeMovementState(BossMovementState.Stop,false);
        animator.Shout();
        yield return new WaitForSeconds(3f);

        //타겟이 있던 위치로 빠르게 전진
        ChangeMovementState(BossMovementState.Skill);
        
        //스킬공격 재생, 멈춘상태
        while (true)
        {
            yield return null;
            //movement.CheckIsDone(out state);
            if (movement.CheckIsDone(out MovementState))
            {
                ChangeMovementState(BossMovementState.Stop,false);
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
        yield return null;
    }

    public void StartAttackEvent(int num)
    {
        if (num >= attackCollider.Length)
        {
            attackCollider[1].enabled = true;
            attackCollider[2].enabled = true;
        }
        else
        {
            attackCollider[num].enabled = true;
        }
    }
    public void EndAttackEvent(int num)
    {
        if (num >= attackCollider.Length)
        {
            attackCollider[1].enabled = false;
            attackCollider[2].enabled = false;
        }
        else
        {
            attackCollider[num].enabled = false;
        }
    }

    public void OnHit()
    {
        bossCombat.Attack(targetStats);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

