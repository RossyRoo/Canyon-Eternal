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
        enemyStats.characterData.enemyWeapons[0].EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        enemyStats.characterData.enemyWeapons[0].DisableDamageCollider();
    }

    #endregion

    public void Footstep()
    {

    }
}
