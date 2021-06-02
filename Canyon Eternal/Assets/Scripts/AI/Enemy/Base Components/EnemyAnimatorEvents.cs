using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorEvents : CharacterAnimatorEvents
{
    EnemyManager enemyManager;


    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    #region Melee

    public void OpenDamageCollider()
    {
        enemyManager.myDamageColliders[0].EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        enemyManager.myDamageColliders[0].DisableDamageCollider();
    }

    #endregion

    public void Footstep()
    {

    }
}
