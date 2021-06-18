using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    EnemyHealthBarUI enemyHealthBarUI;
    public CharacterBarkUI characterBarkUI;

    [HideInInspector] public DamageCollider[] enemyWeapons;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        enemyHealthBarUI = GetComponentInChildren<EnemyHealthBarUI>();
        characterBarkUI = GetComponentInChildren<CharacterBarkUI>();
        enemyWeapons = GetComponentsInChildren<DamageCollider>();
    }

    private void Start()
    {
        currentHealth = characterData.maxHealth;
        enemyHealthBarUI.SetMaxHealth(characterData.maxHealth);

        foreach (var attack in characterData.enemyAttacks)
        {
            if (attack.shortestDistanceNeededToAttack > characterData.attackRange)
                characterData.attackRange = attack.shortestDistanceNeededToAttack;
        }
    }

    public void LoseHealth(int damageHealth, bool isCriticalHit, string damageAnimation = "TakeDamage")
    {
        if (enemyManager.isDead || enemyManager.isInvulnerable)
            return;

        enemyManager.currentTarget = FindObjectOfType<PlayerStats>();

        EnableInvulnerability(characterData.invulnerabilityFrames);

        currentHealth -= damageHealth;

        StartCoroutine(enemyHealthBarUI.SetHealthCoroutine(currentHealth, isCriticalHit, damageHealth));

        enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

        if (!isCriticalHit)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterData.takeNormalDamage);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterData.takeCriticalDamage);
        }

        if (currentHealth <= 0)
        {
            enemyManager.isDead = true;
            currentHealth = 0;
            SFXPlayer.Instance.PlaySFXAudioClip(characterData.deathRattle);
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
    #endregion
}
