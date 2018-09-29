using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[CreateAssetMenu(fileName = "New Monster", menuName = "Enemy/Monster")]

public class SquadUnit : MonoBehaviour {

    //public float lookRadius = 5f;
    SquadTest squadTest;

    [SerializeField]
    Transform target;
    NavMeshAgent agent;

    float distance;
    public bool isEngage;
    CharacterCombat combat;
    public float attackSpeed = 2f;
    float attackCoolDown = 0f;
    float lastAttackTime;

    const float combatCoolDown = 2.0f;

    public float attackDelay = 0.2f;
    Animator animator;

    // Use this for initialization
    void Start () {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        squadTest = GetComponentInParent<SquadTest>();
        animator = GetComponentInChildren<Animator>();
        combat = GetComponentInParent<CharacterCombat>();
    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            animator.SetTrigger("walk");
            distance = Vector3.Distance(target.position, transform.position);
            agent.SetDestination(target.position);
            FaceTarget();
            if (isEngage)
            {
                if (distance <= agent.stoppingDistance)
                {
                    Attack();
                }
            }
        }
        attackCoolDown -= Time.deltaTime;
        

    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void SetEngage(bool engage, Transform target)
    {
        this.target = target;
        isEngage = engage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("enter collider");
            squadTest.Attack();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("exit collider");
            squadTest.ExitRange();
            Debug.Log(squadTest.gameObject);
        }
    }

    public void Attack()
    {
        if (attackCoolDown <= 0f)
        {
            animator.SetTrigger("attack1");

            attackCoolDown = 0.5f / attackSpeed;
            lastAttackTime = Time.time;
        }
    }
    public void Die()
    {
        Debug.Log(gameObject + " 죽음");
        animator.SetTrigger("death1");
    }
    public void Idle()
    {
        animator.SetTrigger("idle2");
    }

}
