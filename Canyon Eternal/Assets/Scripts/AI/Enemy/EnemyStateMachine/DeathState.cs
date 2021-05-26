using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyStateMachine
{
    public float destroyTimer = 0.5f;
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
        FindObjectOfType<PlayerInventory>().AdjustFragmentInventory(enemyStats.fragmentDrop);
        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(destroyTimer);
        Destroy(enemyManager.gameObject);
    }
}
