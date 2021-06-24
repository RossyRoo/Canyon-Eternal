using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    HeartMeter heartMeter;
    StaminaMeter staminaMeter;
    [HideInInspector]public LunchboxMeter lunchboxMeter;

    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    public bool isBuffed = false;

    [Header("Damage SFX")]
    public AudioClip playerDamageSFX;

    [Header("Stamina")]
    public float startingMaxStamina;
    public float currentMaxStamina;
    public float currentStamina;
    public float staminaRecoverySpeed = 1.25f;
    public float staminaRecoveryTimer = 0;
    public float staminaRecoveryBuffer = 0.5f;

    [Header("Lunchbox")]
    public int maxLunchBoxCapacity = 5;
    public int currentLunchBoxCapacity;

    [Header("Completion")]
    public List <int> bossesDefeated;
    public int playerDeathCount;
    

    private void Awake()
    {
        heartMeter = FindObjectOfType<HeartMeter>();
        staminaMeter = FindObjectOfType<StaminaMeter>();
        lunchboxMeter = FindObjectOfType<LunchboxMeter>();
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
    }

    private void Start()
    {
        SetStartingStats();
    }

    public void SetStartingStats()
    {
        currentHealth = characterData.startingMaxHealth;
        characterData.currentMaxHealth = characterData.startingMaxHealth;
        heartMeter.SetMaxHearts(characterData.currentMaxHealth);

        currentStamina = startingMaxStamina;
        currentMaxStamina = startingMaxStamina;
        staminaMeter.SetMaxStamina(currentMaxStamina);

        currentLunchBoxCapacity = maxLunchBoxCapacity;
        lunchboxMeter.SetMaxLunchbox(maxLunchBoxCapacity);
    }

    #region Stamina

    public void LoseStamina(int damageStamina)
    {
        currentStamina -= damageStamina;
        staminaMeter.SetCurrentStamina(currentStamina);

        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    public void RecoverStamina(int recoverStamina)
    {
        currentStamina += recoverStamina;
        staminaMeter.SetCurrentStamina(currentStamina);

        if (currentStamina >= currentMaxStamina)
        {
            currentStamina = currentMaxStamina;
        }
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting || playerManager.isDashing)
        {
            staminaRecoveryTimer = 0;
        }
        else
        {
            staminaRecoveryTimer += Time.deltaTime;
            if (currentStamina < currentMaxStamina && staminaRecoveryTimer > staminaRecoveryBuffer)
            {
                currentStamina += staminaRecoverySpeed * Time.deltaTime;
                staminaMeter.SetCurrentStamina(currentStamina);
            }
        }
    }

    #endregion

    #region Health
    public void LoseHealth(int damageHealth, string damageAnimation = "TakeDamage")
    {
        if (playerManager.isInvulnerable
            || playerManager.isDead)
            return;

        TimeStop timeStop = FindObjectOfType<TimeStop>();

        EnableInvulnerability(characterData.invulnerabilityFrames);
        currentHealth -= damageHealth;
        heartMeter.SetCurrentHealth(currentHealth);

        playerAnimatorHandler.PlayTargetAnimation(damageAnimation, false);
        SFXPlayer.Instance.PlaySFXAudioClip(playerDamageSFX);

        playerParticleHandler.SpawnImpactVFX();
        timeStop.StopTime(0.005f, 10, 0.1f);
        CinemachineManager.Instance.Shake(10f, 0.5f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(playerManager.HandleDeathCoroutine());
        }
    }

    public void RecoverHealth(int recoveryHealth, bool isFullHeal)
    {
        currentHealth += recoveryHealth;
        heartMeter.SetCurrentHealth(currentHealth);

        if(!isFullHeal)
        {
            playerParticleHandler.SpawnHealVFX();

            currentLunchBoxCapacity -= 1;
            lunchboxMeter.SetCurrentLunchBox(currentLunchBoxCapacity);
        }

        if (currentHealth >= characterData.currentMaxHealth)
        {
            currentHealth = characterData.currentMaxHealth;
        }
    }

    #endregion

    #region Handle Buffs

    public void ActivateHealthBuff(int amountToAdjust)
    {
        characterData.currentMaxHealth += amountToAdjust;
        heartMeter.SetMaxHearts(characterData.currentMaxHealth);

        currentHealth += amountToAdjust;

        if (currentHealth >= characterData.currentMaxHealth)
        {
            currentHealth = characterData.currentMaxHealth;
        }

        heartMeter.SetCurrentHealth(currentHealth);
    }

    public void DeactivateHealthBuff()
    {
        characterData.currentMaxHealth = characterData.startingMaxHealth;

        heartMeter.SetMaxHearts(characterData.currentMaxHealth);

        if (currentHealth >= characterData.currentMaxHealth)
        {
            currentHealth = characterData.currentMaxHealth;
        }

        heartMeter.SetCurrentHealth(currentHealth);
    }

    public void ActivateStaminaBuff(int amountToAdjust)
    {
        currentMaxStamina += amountToAdjust;
        staminaMeter.SetMaxStamina(currentMaxStamina);
    }

    public void DeactivateStaminaBuff()
    {
        currentMaxStamina = startingMaxStamina;

        staminaMeter.SetMaxStamina(currentMaxStamina);

        if (currentStamina >= currentMaxStamina)
        {
            currentStamina = currentMaxStamina;
        }

        staminaMeter.SetCurrentStamina(currentStamina);
    }

    #endregion

    #region Invulnerability

    public void EnableInvulnerability(float iFrames)
    {
        playerManager.isInvulnerable = true;
        Invoke("DisableInvulnerability", iFrames);
    }

    public void DisableInvulnerability()
    {
        playerManager.isInvulnerable = false;
    }

    #endregion
}
