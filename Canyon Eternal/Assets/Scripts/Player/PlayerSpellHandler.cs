using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    public Spell activeSpell;

    [Header("Charging")]
    public float currentSpellChargeTime;

    [Header("Projectiles")]
    public GameObject projectilePointerVFXPrefab;
    GameObject projectilePointerVFXGO;
    [HideInInspector]public GameObject currentSpellGO;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
    }


    public void ChargeSpell()
    {
        if (playerStats.currentStamina < activeSpell.staminaCost)
            return;

        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerMeleeHandler.currentMeleeModel.SetActive(false);

        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeAnimation, true);
        playerParticleHandler.SpawnChargeVFX(activeSpell.chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeSFX, 0.05f);


        currentSpellChargeTime = activeSpell.chargeTime;
        playerManager.isChargingSpell = true;
    }

    public void CancelSpell()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.cancelAnimation, false);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.cancelSpell, 0.1f);

        playerManager.isChargingSpell = false;
        playerMeleeHandler.currentMeleeModel.SetActive(true);
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

        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeCompleteAnimation, true);
        playerParticleHandler.SpawnChargeCompleteVFX(activeSpell.chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeCompleteSFX);

        if (activeSpell.isProjectile)
        {
            playerParticleHandler.SpawnCastVFX(activeSpell.GOPrefab);

            projectilePointerVFXGO = Instantiate(projectilePointerVFXPrefab, transform.position, Quaternion.identity);
            projectilePointerVFXGO.transform.parent = transform;
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
            playerMeleeHandler.currentMeleeModel.SetActive(true);
        }

    }

    private void CastProjectile()
    {
        Destroy(projectilePointerVFXGO);

        playerAnimatorHandler.PlayTargetAnimation(activeSpell.castAnimation, false);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.launchSFX);

        currentSpellGO.GetComponent<ProjectilePhysics>().Launch(activeSpell.launchForce, playerManager.lastMoveDirection);
    }

}
