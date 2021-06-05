using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;
    ObjectPool objectPool;

    public Spell activeSpell;

    [Header("Charging")]
    public float currentSpellChargeTime;

    [Header("Projectiles")]
    public GameObject projectilePointerVFXGO;
    public GameObject currentSpellGO;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void Start()
    {
        activeSpell.currentMinDamage = activeSpell.baseMinDamage;
        activeSpell.currentMaxDamage = activeSpell.baseMaxDamage;
    }

    public void ChargeSpell()
    {
        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeAnimation, true);
        playerParticleHandler.SpawnChargeVFX(activeSpell.chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeSFX);


        currentSpellChargeTime = activeSpell.chargeTime;
        playerManager.isChargingSpell = true;
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void CancelSpell()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.cancelAnimation, false);
        playerParticleHandler.SpawnCancelSpellVFX(activeSpell.cancelVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.cancelSFX);


        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerManager.isChargingSpell = false;
    }

    public void TickSpellChargeTimer()
    {
        if (playerManager.isChargingSpell)
        {
            currentSpellChargeTime -= Time.deltaTime;

            if (currentSpellChargeTime < 0)
            {
                currentSpellChargeTime = activeSpell.chargeTime;
                CompleteSpellCharge();
            }
        }
    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(activeSpell.staminaCost);
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeCompleteAnimation, true);
        playerParticleHandler.SpawnChargeCompleteVFX(activeSpell.chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeCompleteSFX);

        if (activeSpell.isProjectile)
        {
            projectilePointerVFXGO.SetActive(true);
            StartCoroutine(RotatePointer());
        }
    }

    private IEnumerator RotatePointer()
    {

        if (playerManager.isCastingSpell && activeSpell.isProjectile)
        {
            int lastMoveDirectionXInt = Mathf.RoundToInt(playerManager.lastMoveDirection.x);
            int lastMoveDirectionYInt = Mathf.RoundToInt(playerManager.lastMoveDirection.y);
            Vector2 lastMoveDirectionInt = new Vector2(lastMoveDirectionXInt, lastMoveDirectionYInt);

            if (lastMoveDirectionInt == Vector2.up)
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else if (lastMoveDirectionInt == Vector2.down)
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else if (lastMoveDirectionInt == Vector2.left)
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
            else if (lastMoveDirectionInt == Vector2.right)
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (lastMoveDirectionInt == new Vector2(-1, 1))
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 135);
            }
            else if (lastMoveDirectionInt == new Vector2(1, 1))
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 45);
            }
            else if (lastMoveDirectionInt == new Vector2(-1, -1))
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 225);
            }
            else if (lastMoveDirectionInt == new Vector2(1, -1))
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 315);
            }
        }
        else
        {
            yield break;
        }


        yield return new WaitForFixedUpdate();
        StartCoroutine(RotatePointer());
    }

    public void HandleAllSpellCasting()
    {
        if(playerManager.isCastingSpell)
        {
            if(activeSpell.isProjectile)
            {
                CastProjectile();
            }

            playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames);
            playerManager.isCastingSpell = false;
        }
    }

    private void CastProjectile()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.castAnimation, false);
        playerParticleHandler.SpawnCastVFX(activeSpell.castVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.castSFX);

        projectilePointerVFXGO.SetActive(false);

        currentSpellGO = Instantiate(activeSpell.spellDamageColliderPrefab, transform.position, Quaternion.identity);
        currentSpellGO.transform.parent = objectPool.transform;
        currentSpellGO.GetComponent<Projectile>().SetSpeedAndDirection(activeSpell.castSpeed, playerManager.lastMoveDirection);
    }

}
