using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    public NavMeshAgent agent;

    [SerializeField]
    public Vector3 target;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}

    private void Update()
    {
        if(target != null && target != Vector3.zero)//
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
        agent.stoppingDistance = 0.5f;
        agent.updateRotation = false;
        
        target = newTarget.transform.position;
    }

    public void StopFollowTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = false;

        //target = null;
    }
    public void FaceTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        Debug.Log(target+" /// "+transform.position+" /// "+(target - transform.position));
        if (direction != Vector3.zero)
        {
            Debug.Log(direction);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); 
        }
    }
}
