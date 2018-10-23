using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable {

    CharacterStats myStats;


    private void Start()
    {
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
