﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadUnit : MonoBehaviour {

    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    public bool isEngage;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                //combatManager.Attack(Player.instance.playerStats);
                FaceTarget();
            }
        }
        if (isEngage)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                //combatManager.Attack(Player.instance.playerStats);
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
