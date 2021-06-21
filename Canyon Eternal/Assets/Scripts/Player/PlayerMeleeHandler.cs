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
    [HideInInspector]
    public GameObject currentMeleeModel;
    public Transform thrustTransform;
    public Transform slashTransform;
    public Transform strikeTransform;
    Transform parentOverride;
    DamageCollider meleeDamageCollider;
    public GameObject meleeMotionVFX;

    [HideInInspector]
    public float currentAttackCooldownTime;
    bool attackMomentumActivated;

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
            parentOverride = thrustTransform;
        }
        else if (activeMeleeCard.isSlash)
        {
            parentOverride = slashTransform;
        }
        else if (activeMeleeCard.isStrike)
        {
            parentOverride = strikeTransform;
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
            if (parentOverride != null)
            {
                meleeModelPrefab.transform.parent = parentOverride;
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
        currentAttackCooldownTime = activeMeleeCard.attackCooldownTime; //START COOLDOWN TIMER

        playerAnimatorHandler.PlayTargetAnimation(activeMeleeCard.attackAnimations[Random.Range(0,activeMeleeCard.attackAnimations.Length)], true); //PLAY ATTACK ANIMATION

        yield return new WaitForSeconds(activeMeleeCard.openDamageColliderBuffer);

        playerStats.LoseStamina(activeMeleeCard.staminaCost); //DRAIN STAMINA
        meleeDamageCollider.EnableDamageCollider(); //ENABLE DAMAGE COLLIDER
        playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames); //START I-FRAMES
        PlayMeleeVFX(); //PLAY SWING SFX AND MOTION VFX

        yield return new WaitForSeconds(activeMeleeCard.closeDamageColliderBuffer); 
        meleeDamageCollider.DisableDamageCollider(); //DISABLE DAMAGE COLLIDER

        attackMomentumActivated = true; //ENABLE ATTACK MOMENTUM
        yield return new WaitForSeconds(0.1f);
        attackMomentumActivated = false; //DISABLE ATTACK MOMENTUM
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.lastMoveDirection * activeMeleeCard.attackMomentum));
        }
    }

    private void PlayMeleeVFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(activeMeleeCard.swingWeaponSFX[Random.Range(0, 2)]);

        Vector3 currentEulerAngles = Vector3.zero;
        Vector3 currentPosition = transform.position;

        #region Get Rotation of Motion VFX
        if (playerManager.lastMoveDirection == new Vector2(0,1))
        {
            currentEulerAngles = new Vector3(0, 0, 0);
            currentPosition = transform.position + new Vector3(0, 4, 0);
        }
        else if(playerManager.lastMoveDirection == new Vector2(0, -1))
        {
            currentEulerAngles = new Vector3(0, 0, 180);
            currentPosition = transform.position + new Vector3(0, -4, 0);
        }
        else if (playerManager.lastMoveDirection == new Vector2(-1, 0))
        {
            currentEulerAngles = new Vector3(0, 0, 90);
            currentPosition = transform.position + new Vector3(-4, 0, 0);
        }
        else if (playerManager.lastMoveDirection == new Vector2(1, 0))
        {
            currentEulerAngles = new Vector3(0, 0, -90);
            currentPosition = transform.position + new Vector3(4, 0, 0);
        }

        #endregion

        GameObject meleeMotionVFXGO = Instantiate(meleeMotionVFX, currentPosition, Quaternion.Euler(currentEulerAngles));
        meleeMotionVFXGO.transform.parent = transform;
        meleeMotionVFXGO.GetComponentInChildren<Animator>().Play(activeMeleeCard.attackAnimations[0]);
        Destroy(meleeMotionVFXGO, 1f);
    }

    public void TickAttackCooldown()
    {
        if(currentAttackCooldownTime > 0)
        {
            currentAttackCooldownTime -= Time.deltaTime;
        }
    }

}