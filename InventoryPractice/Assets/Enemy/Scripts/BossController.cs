using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour {

    public Transform target;
    public float lookRadius;

    NavMeshAgent agent;
    IBossState bossState;
    

    // Use this for initialization
    void Start () {
        Initialize();
	}
    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        //target = PlayerManager.instance.transform;
        bossState = new IBossIdleState();
    }
    // Update is called once per frame
    void Update () {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            //이동으로 상태 변경
            //agent.SetDestination(target.position);
        }
        if(distance<= agent.stoppingDistance)
        {
            //공격으로 상태변경
        }
	}


    void FaceTarget(Vector3 target, Transform my)
    {
        Vector3 directiont = (target - my.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directiont.x, 0, directiont.z));
        my.rotation = Quaternion.Slerp(my.rotation, lookRotation, Time.deltaTime * 5f);
    }
    


}

public interface IBossState
{
    void Behavior();
}
public class IBossIdleState : IBossState
{
    public void Behavior()
    {
        
    }
}
public class IBossAttackState : IBossState
{
    Vector3 target;
    Transform myTransform;
    Animator animator;

    public IBossAttackState(Vector3 target, Transform myTransform, Animator animator)
    {
        this.target = target;
        this.myTransform = myTransform;
        this.animator = animator;
    }
    public void Behavior()
    {
        animator.SetTrigger("Attack01");
        FaceTarget(target, myTransform);
    }

    void FaceTarget(Vector3 target, Transform my)
    {
        Vector3 directiont = (target - my.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directiont.x, 0, directiont.z));
        my.rotation = Quaternion.Slerp(my.rotation, lookRotation, Time.deltaTime * 5f);
    }
}



//public Item item;
//public GameObject particle, itemMesh;

//public override void Interact()
//{
//    base.Interact();
//    PickUp();
//}
//void PickUp()
//{
//    Debug.Log("아이템을 집었다");
//    bool wasPickedUp = InventoryManager.instance.Add(item);

//    if (wasPickedUp)
//    {
//        Vector3 playerPosition = new Vector3(PlayerManager.instance.Player.transform.position.x, PlayerManager.instance.Player.transform.position.y + 1.0f, PlayerManager.instance.Player.transform.position.z);
//        iTween.MoveTo(gameObject, iTween.Hash("position", playerPosition, "easeType", iTween.EaseType.easeInOutSine, "oncomplete", "Destroy", "time", 0.2f));
//    }

//}

//void Destroy()
//{
//    StartCoroutine(DestroyItem());
//}
//IEnumerator DestroyItem()
//{
//    particle.SetActive(true);
//    Destroy(itemMesh);
//    yield return new WaitForSeconds(0.5f);
//    Destroy(gameObject);
//}
