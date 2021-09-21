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
    Transform meleeParentOverride;
    DamageCollider meleeDamageCollider;

    [Header("Weapon Swing VFX")]
    public GameObject meleeMotionVFX;
    public Transform meleeMotionVFXTransform;

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
            if (playerInventory.weaponsInventory.Count > 0)
            {
                playerInventory.activeWeapon = playerInventory.weaponsInventory[0];
            }
        }
        else
        {
            SetMeleeParentOverride();
            LoadMeleeModel();
        }
    }


    public void SetMeleeParentOverride()
    {
        if (playerInventory.activeWeapon.isThrust)
        {
            meleeParentOverride = thrustTransform;
        }
        else if (playerInventory.activeWeapon.isSlash)
        {
            meleeParentOverride = slashTransform;
        }
        else if (playerInventory.activeWeapon.isStrike)
        {
            meleeParentOverride = strikeTransform;
        }
    }

    public void DestroyMeleeModel()
    {
        Destroy(currentMeleeModel);
        currentMeleeModel = null;
    }


    public void LoadMeleeModel()
    {
        GameObject meleeModelPrefab = Instantiate(playerInventory.activeWeapon.modelPrefab) as GameObject;
        if (meleeModelPrefab != null)
        {
            if (meleeParentOverride != null)
            {
                meleeModelPrefab.transform.parent = meleeParentOverride;
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
        playerAnimatorHandler.PlayTargetAnimation(playerInventory.activeWeapon.attackAnimations[Random.Range(0, playerInventory.activeWeapon.attackAnimations.Length)], true); //PLAY ATTACK ANIMATION

        yield return new WaitForSeconds(playerInventory.activeWeapon.openDamageColliderBuffer);

        attackMomentumActivated = true; //ENABLE ATTACK MOMENTUM

        playerStats.LoseStamina(playerInventory.activeWeapon.staminaCost); //DRAIN STAMINA
        meleeDamageCollider.EnableDamageCollider(); //ENABLE DAMAGE COLLIDER
        PlayMeleeVFX(); //PLAY SWING SFX AND MOTION VFX
        SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.activeWeapon.swingWeaponSFX[Random.Range(0, playerInventory.activeWeapon.swingWeaponSFX.Length)], 0.1f);

        yield return new WaitForSeconds(playerInventory.activeWeapon.closeDamageColliderBuffer); 
        meleeDamageCollider.DisableDamageCollider(); //DISABLE DAMAGE COLLIDER
        attackMomentumActivated = false; //DISABLE ATTACK MOMENTUM
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.transform.up * playerInventory.activeWeapon.attackMomentum));
        }
    }
    
    private void PlayMeleeVFX()
    {
        GameObject meleeMotionVFXGO = Instantiate(meleeMotionVFX, meleeMotionVFXTransform.position, transform.rotation);
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