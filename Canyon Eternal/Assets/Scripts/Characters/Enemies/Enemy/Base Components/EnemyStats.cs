using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    EnemyHealthBarUI enemyHealthBarUI;

    [Header("AI Settings")]
    public EnemyAttackAction[] enemyAttacks; //Attacks enemy can use
    public DamageCollider[] myDamageColliders; //Damage colliders on enemy
    public float detectionRadius = 20;      //Distance at which enemy can spot the player
    public float attackRange = 0f;          //Distance enemy needs to enter attack state
    public float evadeRange = 5f;           //Distance enemy will back off target
    public float blindDistance = 50f;       //Distance when enemy loses its target

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        enemyHealthBarUI = GetComponentInChildren<EnemyHealthBarUI>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        enemyHealthBarUI.SetMaxHealth(maxHealth);

        myDamageColliders = GetComponentsInChildren<DamageCollider>();

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            if (enemyAttacks[i].shortestDistanceNeededToAttack > attackRange)
            {
                Debug.Log("SEARCHING ATTACK: " + enemyAttacks[i].name);
                attackRange = enemyAttacks[i].shortestDistanceNeededToAttack;
            }
        }
    }

    public void LoseHealth(int damageHealth, bool isCriticalHit, string damageAnimation = "TakeDamage")
    {
        if (enemyManager.isDead || enemyManager.isInvulnerable)
            return;

        EnableInvulnerability(hurtInvulnerabilityTime);

        currentHealth -= damageHealth;

        StartCoroutine(enemyHealthBarUI.SetHealthCoroutine(currentHealth, isCriticalHit, damageHealth));

        enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

        if (!isCriticalHit)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.takeNormalDamage);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.takeCriticalDamage);
        }

        if (currentHealth <= 0)
        {
            enemyManager.isDead = true;
            currentHealth = 0;
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.deathRattle);
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

    public void DisableAllDamageColliders()
    {
        for (int i = 0; i < myDamageColliders.Length; i++)
        {
            myDamageColliders[i].DisableDamageCollider();
        }
    }

    public void EnableAllDamageColliders()
    {
        for (int i = 0; i < myDamageColliders.Length; i++)
        {
            myDamageColliders[i].EnableDamageCollider();
        }
    }
}
