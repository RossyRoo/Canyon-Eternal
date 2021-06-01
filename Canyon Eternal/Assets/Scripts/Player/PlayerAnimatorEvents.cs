using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;

    PlayerParticleHandler playerParticleHandler;
    Animator animator;

    ObjectPool objectPool;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();
        animator = GetComponent<Animator>();

        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Handle Melee Animation Events

    public void SpawnMelee()
    {
        playerMeleeHandler.currentMeleeModel.SetActive(true);
        GetPlayerAttackDirection();
    }

    public void DespawnMelee()
    {
        if(playerMeleeHandler.comboNumber == 0)
        {
            playerMeleeHandler.currentMeleeModel.SetActive(false);
        }

        playerStats.isChainingInvulnerability = false;
    }

    public void OpenDamageCollider()
    {
        playerMeleeHandler.AddComboDamage();

        playerStats.EnableInvulnerability(playerStats.hurtInvulnerabilityTime);
        playerStats.isChainingInvulnerability = true;

        playerMeleeHandler.meleeDamageCollider.EnableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = true;

        SFXPlayer.Instance.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.meleeWeaponSFXBank.attacks[playerMeleeHandler.comboNumber]);
    }

    public void CloseDamageCollider()
    {
        playerMeleeHandler.RevertComboDamage();

        playerMeleeHandler.meleeDamageCollider.DisableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = false;

        if (playerMeleeHandler.comboNumber >= 2)
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }
    }

    public void EnableComboWindow()
    {
        playerMeleeHandler.canContinueCombo = true;

        if(playerMeleeHandler.comboNumber != 2)
        {
            SpawnComboStar();
        }
    }

    public void DisableComboWindow()
    {
        if(playerMeleeHandler.comboNumber >= 2)
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }

        if (playerMeleeHandler.comboWasHit && !playerMeleeHandler.comboWasMissed)
        {
            playerMeleeHandler.comboNumber++;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }
        else
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }

        playerMeleeHandler.canContinueCombo = false;
        playerMeleeHandler.comboWasHit = false;
        playerMeleeHandler.comboWasMissed = false;
    }

    private void GetPlayerAttackDirection()
    {
        DamageCollider damageCollider = GetComponentInChildren<DamageCollider>();


        if (damageCollider != null)
        {
            damageCollider.knockbackDirection = playerManager.moveDirection;
        }
    }
    #endregion

    public void Footstep()
    {
        int instantiatedFootstepsCount = 0;
        ParticleSystem []instantiatedFootsteps = objectPool.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < instantiatedFootsteps.Length; i++)
        {
            if(instantiatedFootsteps[i].gameObject.name == "footstep_vfx")
            {
                instantiatedFootstepsCount++;
            }
        }

        if(instantiatedFootstepsCount <= 2)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.rockFootsteps
                [Random.Range(0, characterSFXBank.rockFootsteps.Length)]);

            GameObject footstepVFXGO = Instantiate(playerParticleHandler.footstepVFX, playerParticleHandler.mainParticleTransform.position, Quaternion.identity);
            footstepVFXGO.transform.parent = objectPool.transform;
            footstepVFXGO.name = "footstep_vfx";
            Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
        }
        else
        {
            return;
        }

    }

    private void SpawnComboStar()
    {
        playerParticleHandler.currentComboStarGO =
            Instantiate(playerParticleHandler.comboStarVFX,
            new Vector2(playerParticleHandler.critStarTransform.position.x + (playerManager.moveDirection.x * 2), playerParticleHandler.critStarTransform.position.y), Quaternion.identity);

        playerParticleHandler.currentComboStarGO.transform.parent = gameObject.transform;
        Destroy(playerParticleHandler.currentComboStarGO, playerParticleHandler.currentComboStarGO.GetComponent<ParticleSystem>().main.duration);
    }

}
