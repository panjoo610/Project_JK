﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {



    public override void Die()
    {
        base.Die();

        //add ragdooll effect death animation
        Destroy(gameObject,4f);
    }
}
