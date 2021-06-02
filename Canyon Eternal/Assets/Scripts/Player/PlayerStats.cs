using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    HeartMeter heartMeter;
    StaminaMeter staminaMeter;
    public LunchboxMeter lunchboxMeter;

    PlayerManager playerManager;
    PlayerMeleeHandler playerMelee;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    [Header("Stamina")]
    public float maxStamina;
    public float currentStamina;
    public float staminaRecoverySpeed = 1.25f;
    [HideInInspector] public float staminaRecoveryTimer = 0;
    [HideInInspector] public float staminaRecoveryBuffer = 0.5f;

    [Header("Lunchbox")]
    public int maxLunchBoxCapacity = 5;
    public int currentLunchBoxCapacity;

    [Header("I-Frames")]
    public bool isChainingInvulnerability;


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

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
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
            || playerManager.isDead)
            return;


        EnableInvulnerability(hurtInvulnerabilityTime);
        currentHealth -= damageHealth;
        heartMeter.SetCurrentHealth(currentHealth);

        playerAnimatorHandler.PlayTargetAnimation(damageAnimation, false);
        SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.takeNormalDamage);
        CinemachineShake.Instance.Shake(5f, 0.5f);

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
            PlayHealFX();

            currentLunchBoxCapacity -= 1;
            lunchboxMeter.SetCurrentLunchBox(currentLunchBoxCapacity);
        }
        else
        {
            //Do full heal animation and SFX
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void PlayHealFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.consumeHealItem[currentLunchBoxCapacity - 1]);

        GameObject healVFXGO = Instantiate(playerParticleHandler.healVFX, playerParticleHandler.mainParticleTransform.position, Quaternion.identity);
        healVFXGO.GetComponent<ParticleSystemRenderer>().material = playerParticleHandler.healMats[currentLunchBoxCapacity - 1];
        healVFXGO.transform.parent = FindObjectOfType<ObjectPool>().transform;

        Destroy(healVFXGO, healVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SetStartingStats()
    {
        currentHealth = maxHealth;
        heartMeter.SetMaxHearts(maxHealth);

        currentStamina = maxStamina;
        staminaMeter.SetMaxStamina(maxStamina);

        currentLunchBoxCapacity = maxLunchBoxCapacity;
        lunchboxMeter.SetMaxLunchbox(maxLunchBoxCapacity);
    }

    public void EnableInvulnerability(float iFrames)
    {
        playerManager.isInvulnerable = true;
        Invoke("DisableInvulnerability", iFrames);
    }

    public void DisableInvulnerability()
    {
        if(!isChainingInvulnerability)
        {
            playerManager.isInvulnerable = false;
        }
    }


}
