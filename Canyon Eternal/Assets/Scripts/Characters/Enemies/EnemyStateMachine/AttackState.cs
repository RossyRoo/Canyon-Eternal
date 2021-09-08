using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    public EnemyAttackAction currentAttack;

    [Tooltip("Use this until you add acceleration and deceleration to attacks")]
    private float chargeForceMultiplier = 5000f;

    bool attackActivated = false;
    bool attackComplete = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyManager.isDead) //Dies if dead
        {
            return deathState;
        }

        if (enemyManager.isStunned) //Stunned if stunned
        {
            return stunnedState;
        }

        if (attackComplete) //pursues if attack complete
        {
            attackComplete = false;
            attackActivated = false;
            enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return pursueState;
        }

        if (currentAttack == null && !attackActivated) //gets new attack if none is selected and attack hasnt been started
        {
            GetNewAttack(enemyManager, enemyStats);
        }

        if(currentAttack == null) //pursues if no attacks were available
        {
            BreakAttack(enemyManager);
            return pursueState;
        }

        if (currentAttack != null)
        {
            if(!attackActivated)
            {
                StartCoroutine(PerformAttack(enemyManager, enemyStats, enemyAnimatorHandler));
            }

            HandleAttackMomentum(enemyManager);
        }

        return this;

    }


    private void GetNewAttack(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        bool meleeCombatAvailable = false;
        List<EnemyAttackAction> possibleAttacks = new List<EnemyAttackAction>();

        for (int i = 0; i < enemyStats.characterData.enemyAttacks.Length; i++)
        {
            if (distanceFromTarget <= enemyStats.characterData.enemyAttacks[i].shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyStats.characterData.enemyAttacks[i].spaceNeededToStartAttack)
            {
                possibleAttacks.Add(enemyStats.characterData.enemyAttacks[i]);

                if(!enemyStats.characterData.enemyAttacks[i].isRanged)
                {
                    meleeCombatAvailable = true;
                }
            }
        }

        if(meleeCombatAvailable)
        {
            for (int i = 0; i < possibleAttacks.Count; i++)
            {
                if(possibleAttacks[i].isRanged)
                {
                    possibleAttacks.Remove(possibleAttacks[i]);
                }
            }
        }

        if(possibleAttacks.Count > 0)
        {
            currentAttack = possibleAttacks[Random.Range(0, possibleAttacks.Count)];
        }
        else
        {
            currentAttack = null;
        }

    }

    private IEnumerator PerformAttack(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        attackActivated = true;
        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

        yield return new WaitForSeconds(currentAttack.prepareAttackTime); // Charge up the attack (reload if you have a preparation animation

        enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);

        StartCoroutine(HandleBlockVulnerability(enemyManager)); //Enemy is vulnerable to block for 1/2 their attacks recovery time

        if (currentAttack.isRanged)
        {
            HandleRangedAttack(enemyManager, targetDirection);
        }
        else
        {
            StartCoroutine(enemyStats.HandleAttackDamageColliders(currentAttack));
        }


        yield return new WaitForSeconds(currentAttack.prepareAttackTime); // Charge up the attack (reload if you have a preparation animation

        currentAttack = null;
        attackComplete = true;
    }

    private void HandleRangedAttack(EnemyManager enemyManager, Vector3 targetDirection)
    {
        float projectileAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        GameObject projectileGO = Instantiate(currentAttack.projectilePrefab.GOPrefab, transform.position, Quaternion.Euler(Vector3.forward * (projectileAngle + 90f)));
        ProjectilePhysics projectilePhysics = projectileGO.GetComponent<ProjectilePhysics>();

        projectilePhysics.Launch(currentAttack.projectilePrefab.launchForce, targetDirection.normalized);

        SFXPlayer.Instance.PlaySFXAudioClip(currentAttack.projectilePrefab.launchSFX, 0.05f);
    }

    private void HandleAttackMomentum(EnemyManager enemyManager)
    {
        Vector2 chargeDirection = enemyManager.currentTarget.transform.position - transform.position;
        Vector2 chargeForce = (chargeDirection.normalized * currentAttack.chargeForce * Time.deltaTime) * chargeForceMultiplier;

        enemyManager.rb.AddForce(chargeForce);
    }

    public IEnumerator HandleBlockVulnerability(EnemyManager enemyManager)
    {
        enemyManager.isVulnerableToBlock = true;
        yield return new WaitForSeconds(currentAttack.recoveryTime / 2);
        enemyManager.isVulnerableToBlock = false;
    }

    public void BreakAttack(EnemyManager enemyManager)
    {
        if(attackActivated && !attackComplete)
        {
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        }
        
        attackComplete = false;
        attackActivated = false;
        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        currentAttack = null;
        enemyManager.currentState = pursueState;
    }

} 