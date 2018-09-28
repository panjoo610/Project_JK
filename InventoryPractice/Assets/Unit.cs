using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : Interactable {

    public PlayerManager playerManager;
    public CharacterStats Stats;



    // Use this for initialization
    void Start () {
        Debug.Log(gameObject);
        playerManager = PlayerManager.instance;
        Stats = GetComponentInParent<CharacterStats>();
        Debug.Log(Stats);
    }

    public override void Interact()
    {
        Debug.Log(gameObject+" interact");
        base.Interact();

        CharacterCombat playerCombat = playerManager.Player.GetComponent<CharacterCombat>();
        if (playerCombat != null)
        {
            playerCombat.Attack(Stats);
        }
    }
}
