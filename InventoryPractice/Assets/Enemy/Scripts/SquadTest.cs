using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTest : Enemy {

    public int UnitCount;

    public GameObject[] squadMember;
    //public SquadUnit[] squadUnit;
    public List<SquadUnit> squadUnit;
    public Transform[] currentPosition;
    private Vector3 offset;
    Transform target;
    int InRangeCount;
    Collider[] collider;
    public int HitPoint;
    


    private void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        squadMember = new GameObject[UnitCount];
        currentPosition = new Transform[UnitCount];
        collider = new Collider[UnitCount];
        for (int i = 0; i < squadMember.Length; i++)
        {
            squadMember[i] = transform.GetChild(i).gameObject;
            squadUnit.Add(squadMember[i].GetComponent<SquadUnit>());
            currentPosition[i] = new GameObject().transform;
            collider[i] = squadMember[i].gameObject.GetComponent<Collider>();
            
        }
        //gameObject.AddComponent<Collider>();
        SetRandomPosition();
    }

    void SetRandomPosition()
    {
        for (int i = 0; i < currentPosition.Length; i++)
        {
            Vector3 newPosition = (gameObject.transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));
            currentPosition[i].transform.position = newPosition;
        }
    }
    private void LateUpdate()
    {

    }
    Vector3 GetSquadCenter()
    {
        if (squadMember[0] != null && squadMember[1]==null)
        {
            return squadMember[0].transform.position;
        }
        var bounds = new Bounds(squadMember[0].transform.position, Vector3.zero);
        
        return Vector3.zero;
    }

    //hp따라서 유닛 하나씩 없엔다


    public void Attack()
    {
        Debug.Log("Attack");
        InRangeCount += 1;
        Debug.Log(squadUnit.Count);
        foreach(SquadUnit unit in squadUnit)
        {
            Debug.Log("Attack!!");
            unit.SetEngage(true,target);
        }
        //for(int i = 0; i<squadUnit.Count;i++)
        //{
        //    squadUnit[i].SetEngage(true);
        //    Debug.Log("Attack!!");
        //}
    }
    public void Idle()
    {
        SetRandomPosition();
        Debug.Log("Idle");
        for (int i = 0; i < squadUnit.Count; i++)
        {
            squadUnit[i].SetEngage(false,currentPosition[i]);
            Debug.Log("Idle!!");
        }
    }
    public void ExitRange()
    {
        InRangeCount -= 1;
        if (InRangeCount <= 0)
        {
            Idle();
        }
    }

    void Move()
    {

    }

    void SetPosition()
    {

    }

    
}
