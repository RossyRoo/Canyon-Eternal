using System.Collections;
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

    #region Melee

    public void OpenDamageCollider()
    {
        enemyStats.enemyWeapons[0].EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        enemyStats.enemyWeapons[0].DisableDamageCollider();
    }

    #endregion

    public void Footstep()
    {
        //Not doing this
    }

    public void EnableComboWindow()
    {
        //Not doing this
    }

    public void DisableComboWindow()
    {
        //Not doing this
    }
}
