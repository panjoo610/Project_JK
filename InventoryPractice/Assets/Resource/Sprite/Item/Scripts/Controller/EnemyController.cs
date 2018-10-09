using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


    public float lookRadius;

    public Transform target;
    Vector3 originalPos;
    public NavMeshAgent agent;
    [SerializeField]
    CharacterCombat combat;

    [SerializeField]
    GameObject[] enemyObjects;
    [SerializeField]
    Transform[] currentPosition;
    int count;
    private bool isMove;
    private bool isIdle;
    EnemyAnimator enemyAnimator;
    [SerializeField]
    CharacterStats targetStats;

    float idleTime;
    private bool isCoroutine;

    // Use this for initialization
    void Start ()
    {
        originalPos = transform.position;
        agent = gameObject.GetComponent<NavMeshAgent>();
        combat = GetComponent<EnemyCombat>();
        count = GetComponentsInChildren<Animator>().Length;
        enemyAnimator = GetComponent<EnemyAnimator>();
        targetStats = target.GetComponent<CharacterStats>();
        target = PlayerManager.instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            isMove = false;
            agent.SetDestination(target.position);
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                FaceTarget(target.position, enemyObjects[i].transform);
            }
            if (distance <= agent.stoppingDistance)
            {
                
                if (targetStats != null)
                {
                    combat.Attack(targetStats);
                }
            }
        }
        else if(distance>lookRadius)
        {
            agent.SetDestination(originalPos);
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                FaceTarget(originalPos, enemyObjects[i].transform);
            }
            isIdle = true;
        }
        else if (!isCoroutine)
        {
            Debug.Log("StartCoroutine(EnemyObjectMove())");
            StartCoroutine(EnemyObjectMove());
        }
    }

    

    public void RandomMove()
    {
        SetRandomPosition();
        enemyAnimator.PlayWalk(true);
        StartCoroutine(EnemyObjectMove());
    }
    IEnumerator EnemyObjectMove()
    {
        isCoroutine = true;
        yield return new WaitForSeconds(10f);
        SetRandomPosition();
        enemyAnimator.PlayWalk(true);

        float minDistance = .2f;
        int moveCount = 0;
        isMove = true;
        while (isMove==true)
        {
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                enemyObjects[i].transform.localPosition = Vector3.Lerp(enemyObjects[i].transform.localPosition, currentPosition[i].localPosition, agent.speed * Time.deltaTime);

                FaceTarget(currentPosition[i].position, enemyObjects[i].transform);
                if (Vector3.Distance(enemyObjects[i].transform.position, currentPosition[i].position) <= minDistance)
                {
                    enemyAnimator.StopWalk(i);
                    moveCount += 1;
                    if (moveCount >= enemyObjects.Length)
                    {
                        isMove = false;
                        isIdle = true;
                        
                        break;
                    }
                }
                 
            }
            yield return null;
        }
        enemyAnimator.PlayWalk(false);
        isCoroutine = false;
        yield return null;
        
        //enemyAnimator.IEnemyState = new MoveState(enemyAnimator.animators);
    }

    void SetRandomPosition()
    {
        for (int i = 0; i < currentPosition.Length; i++)
        {
            Vector3 newPosition = (gameObject.transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));
            currentPosition[i].transform.position = newPosition;
        }
    }

    void FaceTarget(Vector3 target, Transform my)
    {
        Vector3 directiont = (target - my.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directiont.x, 0, directiont.z));
        my.rotation = Quaternion.Slerp(my.rotation, lookRotation, Time.deltaTime * 5f);
    }




    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
