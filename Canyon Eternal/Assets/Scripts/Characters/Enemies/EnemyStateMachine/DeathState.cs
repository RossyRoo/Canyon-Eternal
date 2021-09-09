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

        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        enemyStats.currentHealth = 0;
        enemyStats.DisableAllDamageColliders();
        enemyManager.DisengagePlayer();

        enemyAnimatorHandler.PlayTargetAnimation("Death", true);
        SFXPlayer.Instance.PlaySFXAudioClip(enemyStats.characterData.deathRattleSFX,0.1f);


        //GIVE STUFF TO PLAYER
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.GetComponent<PlayerInventory>().AdjustFragmentInventory(enemyStats.characterData.fragmentDrop);
        PlayerProgression playerProgression = playerStats.GetComponentInChildren<PlayerProgression>();
        if(!playerProgression.enemiesEncountered.Contains(enemyStats.characterData))
        {
            playerProgression.enemiesEncountered.Add(enemyStats.characterData);
        }
        
        yield return new WaitForSeconds(0.5f);

        Destroy(enemyManager.myWall.gameObject);
        Destroy(enemyStats.gameObject);
    }
}
