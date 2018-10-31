using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour {

    //Player player;

    private void Start()
    {
       // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //player.SetAttackTarget(other.transform);
        }        
    }

}
