using UnityEngine;
using UnityEngine.AI;

public abstract class BossMovement {
    protected Transform target;
    protected Transform myTransform;
    protected float speed;
    protected NavMeshAgent agent;
    protected bool IsDone;


    public abstract void Behaviour();
    public abstract bool CheckIsDone(out BossState bossState);


    protected void FaceTarget(Vector3 target, Transform my)
    {
        Vector3 directiont = (target - my.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directiont.x, 0, directiont.z));
        my.rotation = Quaternion.Slerp(my.rotation, lookRotation, Time.deltaTime * 5f);
    }
    protected virtual void ArriveAtDestination()
    {
        Debug.Log("목적지에 도착");
        IsDone = true;
    }

}
public class NomalMovement : BossMovement
{
    public NomalMovement(NavMeshAgent agent,Transform target, Transform myTransform, float speed)
    {
        base.agent = agent;
        base.target = target;
        base.myTransform = myTransform;
        base.speed = speed;
        IsDone = false;
        agent.stoppingDistance = 1f;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(target.position, myTransform.position);
            if (distance <= agent.stoppingDistance)
            {
                ArriveAtDestination();
            }
            else
            {
                FaceTarget(target.position, myTransform);
                agent.speed = speed;
                agent.SetDestination(target.position);
            } 
        }
    }

    public override bool CheckIsDone(out BossState bossState)
    {
        bossState = BossState.Attack;
        return IsDone;
    }
}


public class StraightMovement : BossMovement
{
    Transform newTransform;
    public StraightMovement(NavMeshAgent agent, Transform target, Transform myTransform, float speed)
    {
        base.agent = agent;
        base.myTransform = myTransform;
        base.speed = speed;
        newTransform = base.myTransform;
        IsDone = false;
        newTransform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        agent.stoppingDistance = 0.3f;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(newTransform.position, myTransform.position);
            if (distance <= agent.stoppingDistance)
            {
                ArriveAtDestination();
            }
            else
            {
                FaceTarget(newTransform.position, myTransform);
                agent.speed = speed;
                agent.SetDestination(newTransform.position);
            } 
        }
    }

    public override bool CheckIsDone(out BossState bossState)
    {
        bossState = BossState.Skill;
        return IsDone;
    }
}

public class IdleMovement : BossMovement
{
    Transform originalTransfrom;
    Transform newTransform;
    public IdleMovement(NavMeshAgent agent, Transform myTransform, float speed)
    {
        base.agent = agent;
        base.myTransform = myTransform;
        base.speed = speed;
        IsDone = false;
        originalTransfrom = base.myTransform;
        newTransform = originalTransfrom;
        newTransform.position = new Vector3(originalTransfrom.position.x - Random.Range(0, 2f), originalTransfrom.position.y, originalTransfrom.position.z - Random.Range(0, 2f));
        agent.stoppingDistance = 0.3f;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(newTransform.position, myTransform.position);
            if (distance <= agent.stoppingDistance)
            {
                ArriveAtDestination();
            }
            else
            {
                FaceTarget(newTransform.position, myTransform);
                agent.speed = speed;
                agent.SetDestination(newTransform.position);
            }
        }
    }

    public override bool CheckIsDone(out BossState bossState)
    {
        bossState = BossState.Idle;
        return IsDone;
    }
}

public class DoNotMovement : BossMovement
{
    public override void Behaviour()
    {
        Debug.Log("움직이지 않음");
        ArriveAtDestination();
    }

    public override bool CheckIsDone(out BossState bossState)
    {
        bossState = BossState.Idle;
        return IsDone;
    }
}

