using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerAnimatorHandler playerAnimatorHandler;

    [Header("Weapon Loading")]
    public MeleeWeapon activeMeleeCard;
    public GameObject currentMeleeModel;
    public Transform thrustTransform;
    public Transform slashTransform;
    public Transform strikeTransform;
    public Transform parrentOverride;
    public DamageCollider meleeDamageCollider;

    [Header("Combo Handling")]
    public bool attackMomentumActivated;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();

        SetParentOverride();
        LoadMelee();
    }


    public void SetParentOverride()
    {
        if (activeMeleeCard.isThrust)
        {
            parrentOverride = thrustTransform;
        }
        else if (activeMeleeCard.isSlash)
        {
            parrentOverride = slashTransform;
        }
        else if (activeMeleeCard.isStrike)
        {
            parrentOverride = strikeTransform;
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

        currentMeleeModel = meleeModelPrefab;
        meleeDamageCollider = currentMeleeModel.GetComponentInChildren<DamageCollider>();
    }

    public IEnumerator HandleMeleeAttack()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeMeleeCard.attackAnimations[Random.Range(0,activeMeleeCard.attackAnimations.Length)], true);

        yield return new WaitForSeconds(activeMeleeCard.openDamageColliderBuffer);

        playerStats.LoseStamina(activeMeleeCard.staminaCost);
        meleeDamageCollider.EnableDamageCollider();
        attackMomentumActivated = true;
        playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames);
        SFXPlayer.Instance.PlaySFXAudioClip(activeMeleeCard.attackSFX[Random.Range(0, 2)]);

        yield return new WaitForSeconds(activeMeleeCard.closeDamageColliderBuffer);

        meleeDamageCollider.DisableDamageCollider();
        attackMomentumActivated = false;
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.lastMoveDirection * activeMeleeCard.attackMomentum));
        }
    }

}