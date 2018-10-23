using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable {

    PlayerManager playerManager;
    CharacterStats myStats;


    private void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<EnemyStats>();
    }
    public override void Interact()
    {
        base.Interact();

        CharacterCombat playerCombat = PlayerManager.instance.Player.GetComponent<CharacterCombat>();

        if (playerCombat != null) //네비메쉬가 작동 중임을 체크할 것
        {
            playerCombat.Attack(myStats);
        }
    }


}
