using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : CharacterAnimator {

    int EnemyCount;
    public Animator[] animators;
    float idleTime;
    bool isIdle1;

    public IEnemyState IEnemyState;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<EnemyCombat>();
        EnemyCount = GetComponentsInChildren<Animator>().Length;//GetComponentsInChildren<GameObject>().Length;
        animators = new Animator[EnemyCount];
        animators = GetComponentsInChildren<Animator>();

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;
        IEnemyState = new IdleState(animators);
    }
    protected override void Update()
    {
        idleTime += Time.deltaTime;
        if (idleTime >= 5f)
        {
            int j = Random.Range(0, animators.Length);
            PlayIdle2(j);
            idleTime = 0f;
        }
        //IEnemyState.AnimatorBehavior();
        float speedPercent = agent.velocity.magnitude / agent.speed;

        if (speedPercent > 0)
        {
            PlayWalk(true);
            isIdle1 = false;
        }
        else if (speedPercent <= 0 || isIdle1 == true)
        {
            PlayWalk(false);
        }
    }
    public void PlayIdle2(int j)
    {
        IEnemyState.AnimatorBehavior(j);
    }
    public void StopWalk(int i)
    {
        IEnemyState.AnimatorBehavior(i);
    }

    public void PlayWalk(bool isWalk)
    {
        if (isWalk)
        {
            IEnemyState = new MoveState(animators);
            isIdle1 = false;
        }
        else
        {
            IEnemyState = new IdleState(animators);
            isIdle1 = true;

        }
    }

    protected override void OnAttack()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("attack1");
        }
    }



}

public interface IEnemyState
{
    void AnimatorBehavior(int i);
}
public class IdleState : IEnemyState
{
    Animator[] animators;
    float idleTime;
    public IdleState(Animator[] animators)
    {
        this.animators = new Animator[animators.Length];
        this.animators = animators;


        for (int i = 0; i < animators.Length; i++)
        {
            this.animators[i].SetBool("walk", false);
        }
    }
    public void AnimatorBehavior(int i)
    {

        animators[i].SetBool("Idle2", true);
    }
}



public class MoveState : IEnemyState
{
    Animator[] animators;
    public MoveState(Animator[] animators)
    {
        this.animators = new Animator[animators.Length];
        this.animators = animators;

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("Idle2", false);
            if (!animators[i].GetBool("walk"))
            {
                Debug.Log("걷는다");
                animators[i].SetBool("walk", true);
            }
        }
    }
    public void AnimatorBehavior(int i)
    {
        animators[i].SetBool("walk", false);
    }
}
