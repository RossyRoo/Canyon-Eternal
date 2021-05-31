using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    EnemyHealthBarUI enemyHealthBarUI;

    [Header("AI Settings")]
    public float rotationSpeed = 25;
    public float detectionRadius = 20;


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
    }

    public void LoseHealth(int damageHealth, string damageAnimation = "TakeDamage")
    {
        if (enemyManager.isDead || enemyManager.isInvulnerable)
            return;

        EnableInvulnerability(hurtInvulnerabilityTime);
        currentHealth -= damageHealth;

        StartCoroutine(enemyHealthBarUI.SetHealthCoroutine(currentHealth, damageHealth));

        enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            enemyManager.isDead = true;
            currentHealth = 0;
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.deathRattle);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.takeDamage);
        }
    }

    public void EnableInvulnerability(float iFrames)
    {
        enemyManager.isInvulnerable = true;
        Invoke("DisableInvulnerability", iFrames);
    }

    private void DisableInvulnerability()
    {
        enemyManager.isInvulnerable = false;
    }
}
