using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerAnimatorHandler playerAnimatorHandler;

    [Header("Weapon Loading")]
    public MeleeWeapon activeMeleeWeapon;
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
        if (activeMeleeWeapon.isThrust)
        {
            parentOverride = thrustTransform;
        }
        else if (activeMeleeWeapon.isSlash)
        {
            parentOverride = slashTransform;
        }
        else if (activeMeleeWeapon.isStrike)
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
        if (activeMeleeWeapon == null)
        {
            return;
        }

        GameObject meleeModelPrefab = Instantiate(activeMeleeWeapon.modelPrefab) as GameObject;
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
        currentAttackCooldownTime = activeMeleeWeapon.attackCooldownTime; //START COOLDOWN TIMER
        meleeDamageCollider.knockbackDirection = playerManager.lastMoveDirection;
        playerAnimatorHandler.PlayTargetAnimation(activeMeleeWeapon.attackAnimations[Random.Range(0,activeMeleeWeapon.attackAnimations.Length)], true); //PLAY ATTACK ANIMATION

        yield return new WaitForSeconds(activeMeleeWeapon.openDamageColliderBuffer);

        playerStats.LoseStamina(activeMeleeWeapon.staminaCost); //DRAIN STAMINA
        meleeDamageCollider.EnableDamageCollider(); //ENABLE DAMAGE COLLIDER
        PlayMeleeVFX(); //PLAY SWING SFX AND MOTION VFX

        yield return new WaitForSeconds(activeMeleeWeapon.closeDamageColliderBuffer); 
        meleeDamageCollider.DisableDamageCollider(); //DISABLE DAMAGE COLLIDER

        attackMomentumActivated = true; //ENABLE ATTACK MOMENTUM
        yield return new WaitForSeconds(0.1f);
        attackMomentumActivated = false; //DISABLE ATTACK MOMENTUM
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.lastMoveDirection * activeMeleeWeapon.attackMomentum));
        }
    }

    private void PlayMeleeVFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(activeMeleeWeapon.swingWeaponSFX[Random.Range(0, activeMeleeWeapon.swingWeaponSFX.Length)]);

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
        meleeMotionVFXGO.GetComponentInChildren<Animator>().Play(activeMeleeWeapon.attackAnimations[0]);
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