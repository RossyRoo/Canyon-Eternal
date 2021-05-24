using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;

    [Header("AI Settings")]
    public float rotationSpeed = 25;
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void LoseHealth(int damageHealth, string damageAnimation = "TakeDamage")
    {
        if (enemyManager.isDead || enemyManager.isInvulnerable)
            return;

        currentHealth -= damageHealth;

        enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            enemyManager.isDead = true;
            currentHealth = 0;
        }
    }
}
