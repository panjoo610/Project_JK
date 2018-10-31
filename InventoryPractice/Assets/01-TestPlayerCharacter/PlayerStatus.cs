using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour {
    [SerializeField]
    int playerHp;
    public int PlayerHp
    {
        get { return playerHp; }
        set { playerHp =value; }
    }
    [SerializeField]
    int playerExp;
    public int PlayerExp
    {
        get { return playerExp; }
        set { playerExp = value; }
    }
    [SerializeField]
    int playerAttackPower;
    public int PlayerAttackPower
    {
        get { return playerAttackPower; }
        set { playerAttackPower = value; }
    }
    [SerializeField]
    int playerDefensivePower;
    public int PlayerDefensivePower
    {
        get { return playerDefensivePower; }
        set { playerDefensivePower = value; }
    }

    void Start () {
        playerHp = 300;
        PlayerExp = 0;
        playerAttackPower = 10;
        playerDefensivePower = 10;
	}
	

}
