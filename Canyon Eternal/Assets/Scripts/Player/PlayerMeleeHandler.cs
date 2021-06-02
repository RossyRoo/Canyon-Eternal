using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerStats playerStats;
    PlayerParticleHandler playerParticleHandler;
    Animator animator;

    [Header("Weapon Loading")]
    public MeleeCard activeMeleeCard;
    public GameObject currentMeleeModel;
    public Transform thrustTransform;
    public Transform slashTransform;
    public Transform strikeTransform;
    Transform parrentOverride;
    public DamageCollider meleeDamageCollider;

    [Header("Combo Handling")]
    public int comboNumber = 0;
    public bool canContinueCombo;
    public bool attackMomentumActivated;
    public bool comboWasHit;
    public bool comboWasMissed;
    public AudioClip [] comboSFX;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
        animator = GetComponentInChildren<Animator>();

        SetParentOverride();
        LoadMelee();
    }


    public void SetParentOverride()
    {
        if(activeMeleeCard.isThrust)
        {
            parrentOverride = thrustTransform;
        }
        else if(activeMeleeCard.isSlash)
        {
            parrentOverride = slashTransform;
        }
        else if(activeMeleeCard.isStrike)
        {
            parrentOverride = strikeTransform;
        }
        else
        {
            Debug.LogWarning("You do not have a melee weapon assigned to the player.");
        }
    }

    public void UnloadMelee()
    {
        if (currentMeleeModel != null)
        {
            currentMeleeModel.SetActive(false);
        }
    }

    public void DestroyMelee()
    {
        Destroy(currentMeleeModel);
        currentMeleeModel = null;
    }

    public void LoadMelee()
    {
        if (activeMeleeCard == null)
        {
            UnloadMelee();
            return;
        }

        GameObject meleeModelPrefab = Instantiate(activeMeleeCard.modelPrefab) as GameObject;
        if (meleeModelPrefab != null)
        {
            if (parrentOverride != null)
            {
                meleeModelPrefab.transform.parent = parrentOverride;
            }
            else
            {
                meleeModelPrefab.transform.parent = transform;
            }
            meleeModelPrefab.transform.localPosition = Vector3.zero;
            meleeModelPrefab.transform.localRotation = Quaternion.identity;
            meleeModelPrefab.transform.localScale = Vector3.one;
        }

        activeMeleeCard.currentMinDamage = activeMeleeCard.baseMinDamage;
        activeMeleeCard.currentMaxDamage = activeMeleeCard.baseMaxDamage;

        currentMeleeModel = meleeModelPrefab;
        meleeDamageCollider = currentMeleeModel.GetComponentInChildren<DamageCollider>();
        meleeModelPrefab.SetActive(false);
    }

    public void BeginNewAttackChain()
    {
        if (playerStats.currentStamina < activeMeleeCard.staminaCost)
            return;

        GetPlayerAttackDirection();

        Invoke("SpawnMeleeWeapon", 0.1f);
        
        playerAnimatorHandler.PlayTargetAnimation(activeMeleeCard.attackAnimation, true);
    }

    private void SpawnMeleeWeapon()
    {
        //Do Particle FX
        currentMeleeModel.SetActive(true);
    }

    public void GetPlayerAttackDirection()
    {
        DamageCollider damageCollider = GetComponentInChildren<DamageCollider>();

        if (damageCollider != null)
        {
            damageCollider.knockbackDirection = playerManager.moveDirection;
        }
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            if (comboNumber == 1)
            {
                if(playerManager.moveDirection != Vector2.zero)
                {
                    playerManager.rb.AddForce((playerManager.moveDirection * activeMeleeCard.attackMomentum) * 2);
                }
                else
                {
                    playerManager.rb.AddForce((playerManager.lastMoveDirection * activeMeleeCard.attackMomentum) * 2);
                }
            }
            else if (comboNumber == 2)
            {
                if (playerManager.moveDirection != Vector2.zero)
                {
                    playerManager.rb.AddForce((playerManager.moveDirection * activeMeleeCard.attackMomentum) * 3);
                }
                else
                {
                    playerManager.rb.AddForce((playerManager.lastMoveDirection * activeMeleeCard.attackMomentum) * 3);
                }
            }
        }
    }

    public void AddComboDamage()
    {
        if(comboNumber == 0)
        {
            RevertComboDamage();
        }
        else if(comboNumber == 1)
        {
            activeMeleeCard.currentMinDamage = activeMeleeCard.baseMinDamage + activeMeleeCard.comboDamageToAdd;
            activeMeleeCard.currentMaxDamage = activeMeleeCard.baseMaxDamage + activeMeleeCard.comboDamageToAdd;
        }
        else if(comboNumber == 2)
        {
            activeMeleeCard.currentMinDamage = activeMeleeCard.baseMinDamage + (activeMeleeCard.comboDamageToAdd * 2);
            activeMeleeCard.currentMaxDamage = activeMeleeCard.baseMaxDamage + (activeMeleeCard.comboDamageToAdd * 2);
        }
    }

    public void RevertComboDamage()
    {
        activeMeleeCard.currentMinDamage = activeMeleeCard.baseMinDamage;
        activeMeleeCard.currentMaxDamage = activeMeleeCard.baseMaxDamage;
    }

    public void PlayComboHitSFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(comboSFX[comboNumber], 0.3f, 0.2f);
    }

    public void HandleComboAttempt()
    {
        playerParticleHandler.ChangeStarToYellow();

        if (playerManager.isAttacking)
        {
            if (!canContinueCombo && !comboWasHit && !comboWasMissed)
            {
                //MISS
                canContinueCombo = false;
                animator.SetBool("comboWasHit", true);

                if (comboNumber != 2)
                {
                    animator.SetBool("comboWasMissed", true);
                    playerStats.LoseStamina(activeMeleeCard.staminaCost);
                }

                playerParticleHandler.ChangeStarToRed();
            }
            else if (canContinueCombo && !comboWasMissed)
            {
                //HIT
                canContinueCombo = false;
                animator.SetBool("comboWasHit", true);
                GetPlayerAttackDirection();
                playerParticleHandler.ChangeStarToGreen();
                PlayComboHitSFX();
            }

            if(comboNumber == 2)
            {
                animator.SetBool("comboWasHit", true);
            }

            playerParticleHandler.ChangeStarToYellow();
        }
    }

}
