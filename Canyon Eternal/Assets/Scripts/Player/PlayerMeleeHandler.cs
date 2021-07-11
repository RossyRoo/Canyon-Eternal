using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;
    PlayerAnimatorHandler playerAnimatorHandler;

    [Header("Weapon Loading")]
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
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();


        if (playerInventory.activeWeapon == null)
        {
            if (playerInventory.weaponsInventory.Count != 0)
            {
                playerInventory.activeWeapon = playerInventory.weaponsInventory[0];
            }
        }

        if(playerInventory.activeWeapon != null)
        {
            SetParentOverride();
            LoadMelee();
        }

    }


    public void SetParentOverride()
    {
        if (playerInventory.activeWeapon.isThrust)
        {
            parentOverride = thrustTransform;
        }
        else if (playerInventory.activeWeapon.isSlash)
        {
            parentOverride = slashTransform;
        }
        else if (playerInventory.activeWeapon.isStrike)
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
        GameObject meleeModelPrefab = Instantiate(playerInventory.activeWeapon.modelPrefab) as GameObject;
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
        currentAttackCooldownTime = playerInventory.activeWeapon.attackCooldownTime; //START COOLDOWN TIMER
        meleeDamageCollider.knockbackDirection = playerManager.lastMoveDirection;
        playerAnimatorHandler.PlayTargetAnimation(playerInventory.activeWeapon.attackAnimations[Random.Range(0, playerInventory.activeWeapon.attackAnimations.Length)], true); //PLAY ATTACK ANIMATION

        yield return new WaitForSeconds(playerInventory.activeWeapon.openDamageColliderBuffer);

        playerStats.LoseStamina(playerInventory.activeWeapon.staminaCost); //DRAIN STAMINA
        meleeDamageCollider.EnableDamageCollider(); //ENABLE DAMAGE COLLIDER
        PlayMeleeVFX(); //PLAY SWING SFX AND MOTION VFX

        yield return new WaitForSeconds(playerInventory.activeWeapon.closeDamageColliderBuffer); 
        meleeDamageCollider.DisableDamageCollider(); //DISABLE DAMAGE COLLIDER

        attackMomentumActivated = true; //ENABLE ATTACK MOMENTUM
        yield return new WaitForSeconds(0.1f);
        attackMomentumActivated = false; //DISABLE ATTACK MOMENTUM
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.lastMoveDirection * playerInventory.activeWeapon.attackMomentum));
        }
    }

    private void PlayMeleeVFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.activeWeapon.swingWeaponSFX[Random.Range(0, playerInventory.activeWeapon.swingWeaponSFX.Length)]);

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
        meleeMotionVFXGO.GetComponentInChildren<Animator>().Play(playerInventory.activeWeapon.attackAnimations[0]);
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