using UnityEngine;
using UnityEngine.AI;

public abstract class BossMovement {
    protected Transform target;
    protected Transform myTransform;
    protected float speed;
    protected NavMeshAgent agent;
    protected bool IsDone;


    public abstract void Behaviour();
    public abstract bool CheckIsDone(out BossMovementState bossState);


    protected void FaceTarget(Vector3 target, Transform my)
    {
        Vector3 directiont = (target - my.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directiont.x, 0, directiont.z));
        my.rotation = Quaternion.Slerp(my.rotation, lookRotation, Time.deltaTime * 5f);
    }
    protected virtual void ArriveAtDestination()
    {
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
        base.agent.stoppingDistance = 2f;
        base.agent.speed = speed;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(target.position, myTransform.position);
            if (distance <= agent.stoppingDistance+1)
            {
                FaceTarget(target.position, myTransform);
                ArriveAtDestination();
            }
            else
            {
                FaceTarget(target.position, myTransform);
                agent.SetDestination(target.position);
            } 
        }
    }

    public override bool CheckIsDone(out BossMovementState bossState)
    {
        bossState = BossMovementState.Move;
        return IsDone;
    }
}


public class StraightMovement : BossMovement
{
    Vector3 newTransform;
    public StraightMovement(NavMeshAgent agent, Transform target, Transform myTransform, float speed)
    {
        base.agent = agent;
        base.myTransform = myTransform;
        base.speed = speed;
        IsDone = false;
        newTransform = new Vector3(target.position.x, target.position.y, target.position.z);
        agent.stoppingDistance = 2f;
        base.agent.speed = speed;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(newTransform, myTransform.position);
            Debug.Log(distance <= agent.stoppingDistance);
            if (distance <= agent.stoppingDistance+1f)
            {
                
                ArriveAtDestination();
            }
            else
            {
                FaceTarget(newTransform, myTransform);
                agent.SetDestination(newTransform);
            } 
        }
    }

    public override bool CheckIsDone(out BossMovementState bossState)
    {
        
        bossState = BossMovementState.Skill;
        return IsDone;
    }
    
}

public class IdleMovement : BossMovement
{
    Transform originalTransfrom;
    Vector3 newTransform;
    float StayTime;
    public IdleMovement(NavMeshAgent agent, Transform myTransform, float speed)
    {
        base.agent = agent;
        base.myTransform = myTransform;
        base.speed = speed;
        IsDone = false;
        originalTransfrom = base.myTransform;
        newTransform = new Vector3(originalTransfrom.position.x - Random.Range(-2f, 2f), originalTransfrom.position.y, originalTransfrom.position.z - Random.Range(-2f, 2f));
        //newTransform.position = new Vector3(originalTransfrom.position.x - Random.Range(0, 2f), originalTransfrom.position.y, originalTransfrom.position.z - Random.Range(0, 2f));
        agent.stoppingDistance = 0.3f;
        base.agent.speed = speed;
        StayTime = 0f;
    }

    public override void Behaviour()
    {
        if (!IsDone)
        {
            float distance = Vector3.Distance(newTransform, myTransform.position);
            if (distance <= agent.stoppingDistance)
            {
                StayTime += Time.deltaTime;
                if (StayTime >= 10f)
                {
                    ArriveAtDestination(); 
                }
            }
            else
            {
                FaceTarget(newTransform, myTransform);
                agent.SetDestination(newTransform);
            }
        }
    }

    public override bool CheckIsDone(out BossMovementState bossState)
    {
        bossState = BossMovementState.Idle;
        return IsDone;
    }
}

public class DoNotMovement : BossMovement
{
    bool isLooking;
    public DoNotMovement(NavMeshAgent agent, bool IsLooking, Transform myTransform = null, Transform target = null)
    {
        isLooking = IsLooking;
        base.agent = agent;
        base.myTransform = myTransform;
        base.target = target;
        agent.speed = 1;
        agent.SetDestination(myTransform.position);
    }
    public override void Behaviour()
    {
        if (isLooking)
        {
            FaceTarget(target.position, myTransform);
        }
        ArriveAtDestination();
    }

    public override bool CheckIsDone(out BossMovementState bossState)
    {
        bossState = BossMovementState.Stop;
        return IsDone;
    }
}

