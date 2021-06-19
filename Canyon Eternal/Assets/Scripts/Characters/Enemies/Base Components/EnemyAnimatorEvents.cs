﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorEvents : CharacterAnimatorEvents
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;


    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public void Footstep()
    {
        //Not doing this
    }

}
