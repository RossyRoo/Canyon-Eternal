using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    EnemyAttackAction currentAttack;

    [Tooltip("Use this until you add acceleration and deceleration to attacks")]
    private float chargeForceMultiplier = 5000f;


    bool attackActivated = false;
    bool attackComplete = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        //MIGHT DIE
        if (enemyManager.isDead)
        {
            return deathState;
        }

        //MIGHT BECOME STUNNED
        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        if (attackComplete)
        {
            attackComplete = false;
            attackActivated = false;
            return pursueState;
        }

        if (currentAttack == null && !attackActivated)
        {
            GetNewAttack(enemyManager, enemyStats);
        }

        if (currentAttack != null && !attackActivated)
        {
            StartCoroutine(PerformAttack(enemyManager, enemyStats, enemyAnimatorHandler));
        }

        return this;

    }

    private void GetNewAttack(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyStats.characterData.enemyAttacks.Length; i++)
        {
            if (distanceFromTarget <= enemyStats.characterData.enemyAttacks[i].shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyStats.characterData.enemyAttacks[i].spaceNeededToStartAttack)
            {
                maxScore += enemyStats.characterData.enemyAttacks[i].attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyStats.characterData.enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyStats.characterData.enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.spaceNeededToStartAttack)
            {
                if (currentAttack != null)
                    return;

                temporaryScore += enemyAttackAction.attackScore;

                if (temporaryScore > randomValue)
                {
                    currentAttack = enemyAttackAction;
                }
            }
        }

    }

    private IEnumerator PerformAttack(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        attackActivated = true;
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

        yield return new WaitForSeconds(currentAttack.prepareAttackTime); // Charge up the attack (reload if you have a preparation animation

        enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyAnimatorHandler.UpdateFloatAnimationValues(targetDirection.normalized.x, targetDirection.normalized.y, false);

        StartCoroutine(enemyStats.HandleBlockVulnerability(currentAttack));


        if (!currentAttack.isRanged)
        {
            HandleMeleeAttack(enemyStats, enemyManager, targetDirection);
        }
        else
        {
            HandleRangedAttack(enemyManager, targetDirection);
        }

        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

        yield return new WaitForSeconds(currentAttack.prepareAttackTime); // Charge up the attack (reload if you have a preparation animation

        currentAttack = null;
        attackComplete = true;
    }

    private void HandleRangedAttack(EnemyManager enemyManager, Vector3 targetDirection)
    {
        enemyManager.rb.velocity = Vector3.zero;

        float projectileAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        GameObject projectileGO = Instantiate(currentAttack.projectilePrefab.GOPrefab, transform.position, Quaternion.Euler(Vector3.forward * (projectileAngle + 90f)));
        ProjectilePhysics projectilePhysics = projectileGO.GetComponent<ProjectilePhysics>();

        projectilePhysics.Launch(currentAttack.projectilePrefab.launchForce, targetDirection.normalized);

        SFXPlayer.Instance.PlaySFXAudioClip(currentAttack.projectilePrefab.launchSFX, 0.05f);
    }

    private void HandleMeleeAttack(EnemyStats enemyStats, EnemyManager enemyManager, Vector3 targetDirection)
    {
        StartCoroutine(enemyStats.HandleAttackDamageColliders(currentAttack));

        if (currentAttack.chargeForce != 999f)
        {
            Vector2 force = (targetDirection.normalized * currentAttack.chargeForce * Time.deltaTime) * chargeForceMultiplier;
            enemyManager.rb.AddForce(force);
        }
    }


} 