﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyStateMachine
{
    public bool runningDeathCoroutine;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (!runningDeathCoroutine)
        {
            StartCoroutine(DeathCoroutine(enemyManager, enemyStats, enemyAnimatorHandler));
        }

        return this;
    }

    private IEnumerator DeathCoroutine(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        runningDeathCoroutine = true;

        enemyStats.DisableAllDamageColliders();
        enemyManager.DisengagePlayer();

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.bossesDefeated.Add(enemyStats.characterData.bossID);
        playerStats.GetComponent<PlayerInventory>().AdjustFragmentInventory(enemyStats.characterData.fragmentDrop);

        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        enemyAnimatorHandler.PlayTargetAnimation("Death", true);
        
        yield return new WaitForSeconds(1f);
        Destroy(enemyManager.myWall.gameObject);
        Destroy(enemyManager.gameObject);
    }
}
