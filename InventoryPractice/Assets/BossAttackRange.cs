using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRange : MonoBehaviour {

    //public event System.Action OnAttack;
    BossController bossController;

    private void Start()
    {
        //OnAttack += BossAttackRange_OnAttack;
        bossController = GetComponentInParent<BossController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //BossAttackRange_OnAttack();
        //Debug.Log("콜라이더힛");
        bossController.OnHit();
    }

    //private void BossAttackRange_OnAttack()
    //{
    //    throw new System.NotImplementedException();
    //}
}
