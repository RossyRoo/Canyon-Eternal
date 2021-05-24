using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    HeartMeter heartMeter;
    StaminaMeter staminaMeter;

    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;

    public float staminaRecoverySpeed = 1.25f;
    [HideInInspector] public float staminaRecoveryTimer = 0;
    [HideInInspector] public float staminaRecoveryBuffer = 0.5f;

    private void Awake()
    {
        heartMeter = FindObjectOfType<HeartMeter>();
        staminaMeter = FindObjectOfType<StaminaMeter>();
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        heartMeter.SetMaxHearts(maxHealth);

        currentStamina = maxStamina;
        staminaMeter.SetMaxStamina(maxStamina);
    }

    public void LoseStamina(int damageStamina)
    {
        currentStamina -= damageStamina;
        staminaMeter.SetCurrentStamina(currentStamina);

        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRecoveryTimer = 0;
        }
        else
        {
            staminaRecoveryTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRecoveryTimer > staminaRecoveryBuffer)
            {
                currentStamina += staminaRecoverySpeed * Time.deltaTime;
                staminaMeter.SetCurrentStamina(currentStamina);
            }
        }
    }

    public void LoseHealth(int damageHealth, string damageAnimation = "TakeDamage")
    {
        if (playerManager.isInvulnerable
//            || playerManager.isDecidingFate
            || playerManager.isDead)
            return;


        currentHealth -= damageHealth;
        heartMeter.SetCurrentHealth(currentHealth);

        playerAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
        playerManager.sFXPlayer.PlaySFXAudioClip(characterSFXBank.takeDamage);
        CinemachineShake.Instance.Shake(5f, 0.5f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //playerAnimatorHandler.PlayTargetAnimation("DeathKneel", true);
            //HUD hud = FindObjectOfType<HUD>();

            //hud.DisplayDeathDecision();
            //playerManager.isDecidingFate = true;
        }
    }

    public void RecoverHealth(int recoveryHealth)
    {
        currentHealth += recoveryHealth;
        heartMeter.SetCurrentHealth(currentHealth);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void EnableInvulnerability(float iFrames)
    {
        playerManager.isInvulnerable = true;
        Invoke("DisableInvulnerability", iFrames);
    }

    private void DisableInvulnerability()
    {
        playerManager.isInvulnerable = false;
    }
}
