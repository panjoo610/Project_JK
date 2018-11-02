using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour {


    public void OnAttackColliderIn()
    {
        
        //enemyCombat.Attack(targetStats);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("dfda" + collision.collider.name);
    }
}
