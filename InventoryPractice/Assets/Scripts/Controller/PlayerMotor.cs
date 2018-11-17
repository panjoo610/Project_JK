using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    public NavMeshAgent agent;

    [SerializeField]
    public Transform target;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}

    private void Update()
    {
        if (target != null && target.tag != "Item")//
        {
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = 1f;
        agent.updateRotation = true;
        
        target = newTarget.transform;
    }

    public void StopFollowTarget()
    {
        agent.stoppingDistance = 1f;
        agent.updateRotation = false;

        target = null;
    }
    public void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        //Debug.Log(target+" /// "+transform.position+" /// "+(target - transform.position));
        
        if (direction != Vector3.zero)
        {
            //Debug.Log(direction);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); 
        }
    }
}
