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
            StartCoroutine(DeathCoroutine(enemyManager, enemyStats));
        }

        return this;
    }

    private IEnumerator DeathCoroutine(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        runningDeathCoroutine = true;

        enemyStats.DisableAllDamageColliders();

        FindObjectOfType<PlayerInventory>().AdjustFragmentInventory(enemyStats.fragmentDrop);

        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);
        Destroy(enemyManager.myWall.gameObject);
        Destroy(enemyManager.gameObject);
    }
}
