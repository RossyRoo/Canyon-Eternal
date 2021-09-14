using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    public EnemyHealthBarUI enemyHealthBarUI;
    AttackState attackState;

    public CharacterBarkUI characterBarkUI;
    DamageCollider[] enemyWeapons;


    private void Awake()
    {
        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        attackState = GetComponentInChildren<AttackState>();
        enemyWeapons = GetComponentsInChildren<DamageCollider>();
    }

    private void Start()
    {
        currentHealth = characterData.startingMaxHealth;
        enemyHealthBarUI.SetMaxHealth(characterData.startingMaxHealth);

        foreach (var attack in characterData.enemyAttacks)
        {
            if (attack.shortestDistanceNeededToAttack > characterData.attackRange)
                characterData.attackRange = attack.shortestDistanceNeededToAttack;
        }
    }

    public void LoseHealth(float damageHealth, bool isCriticalHit, string damageAnimation = "TakeDamage")
    {
        if (enemyManager.isDead || enemyManager.isInvulnerable)
            return;

        enemyManager.currentTarget = FindObjectOfType<PlayerStats>();

        EnableInvulnerability(characterData.invulnerabilityFrames);

        currentHealth -= damageHealth;
        attackState.BreakAttack(enemyManager);

        StartCoroutine(enemyHealthBarUI.SetHealthCoroutine(true, currentHealth, isCriticalHit, damageHealth));

        enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
        CinemachineManager.Instance.Shake(7f, 0.25f);

        if (currentHealth <= 0)
        {
            enemyManager.isDead = true;
        }

    }

    #region Invulnerability
    public void EnableInvulnerability(float iFrames)
    {
        enemyManager.isInvulnerable = true;
        Invoke("DisableInvulnerability", iFrames);
    }

    private void DisableInvulnerability()
    {
        enemyManager.isInvulnerable = false;
    }
    #endregion

    #region Enable/Disable Damage Colliders

    public void DisableAllDamageColliders()
    {
        for (int i = 0; i < enemyWeapons.Length; i++)
        {
            enemyWeapons[i].DisableDamageCollider();
        }
    }

    public void EnableAllDamageColliders()
    {
        for (int i = 0; i < enemyWeapons.Length; i++)
        {
            enemyWeapons[i].EnableDamageCollider();
        }
    }

    public IEnumerator HandleAttackDamageColliders(EnemyAttackAction enemyAttackAction)
    {
        yield return new WaitForSeconds(enemyAttackAction.openDamageColliderBuffer);
        enemyWeapons[0].EnableDamageCollider();

        yield return new WaitForSeconds(enemyAttackAction.closeDamageColliderBuffer);
        enemyWeapons[0].DisableDamageCollider();
    }

    #endregion


}
