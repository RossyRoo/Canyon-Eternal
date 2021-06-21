using System.Collections;
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
        playerStats.GetComponent<PlayerInventory>().AdjustFragmentInventory(enemyStats.characterData.fragmentDrop);

        if (enemyStats.characterData.isBoss)
        {
            playerStats.bossesDefeated.Add(enemyStats.characterData.enemyID);
        }

        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        enemyAnimatorHandler.PlayTargetAnimation("Death", true);
        
        yield return new WaitForSeconds(0.5f);
        Destroy(enemyManager.myWall.gameObject);
        Destroy(enemyManager.gameObject);
    }
}
