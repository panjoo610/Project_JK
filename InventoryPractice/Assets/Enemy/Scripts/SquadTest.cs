using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTest : MonoBehaviour {

    int hp;

    public GameObject[] squadMember;
    public SquadUnit[] squadUnit;
    public Transform[] currentPosition;
    private Vector3 offset;

    private void Start()
    {
        squadMember = new GameObject[4];
        for (int i = 0; i < squadMember.Length; i++)
        {
            squadMember[i] = transform.GetChild(i).gameObject; 
        }
        squadUnit = gameObject.GetComponentsInChildren<SquadUnit>();
        
    }
    private void LateUpdate()
    {

        Vector3 centerPositon = GetSquadCenter();
        Vector3 newPositon = centerPositon + offset;

        //gameObject.transform.position = newPositon;
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


    void Attack()
    {
        
    }

    void Move()
    {

    }

    void SetPosition()
    {

    }

}
