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


        if (playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] != null)
        {
            SetMeleeParentOverride();
            LoadMeleeModel();
        }

    }


    public void SetMeleeParentOverride()
    {
        if (playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].isThrust)
        {
            meleeParentOverride = thrustTransform;
        }
        else if (playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].isSlash)
        {
            meleeParentOverride = slashTransform;
        }
        else if (playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].isStrike)
        {
            meleeParentOverride = strikeTransform;
        }
    }

    public void DestroyMeleeModel()
    {
        if(currentMeleeModel != null)
        {
            Destroy(currentMeleeModel);
            currentMeleeModel = null;
            meleeDamageCollider = null;
        }
    }


    public void LoadMeleeModel()
    {
        DestroyMeleeModel();

        GameObject meleeModelPrefab = Instantiate(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].modelPrefab) as GameObject;
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
        meleeDamageCollider.FindComponents();
    }

    public IEnumerator HandleMeleeAttack()
    {
        currentAttackCooldownTime = playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].attackCooldownTime; //START COOLDOWN TIMER
        playerAnimatorHandler.PlayTargetAnimation(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].attackAnimations[Random.Range(0, playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].attackAnimations.Length)], true); //PLAY ATTACK ANIMATION

        yield return new WaitForSeconds(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].openDamageColliderBuffer);

        attackMomentumActivated = true; //ENABLE ATTACK MOMENTUM

        playerStats.LoseStamina(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].staminaCost); //DRAIN STAMINA
        meleeDamageCollider.EnableDamageCollider(); //ENABLE DAMAGE COLLIDER
        PlayMeleeVFX(); //PLAY SWING SFX AND MOTION VFX
        SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].swingWeaponSFX[Random.Range(0, playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].swingWeaponSFX.Length)], 0.1f);

        yield return new WaitForSeconds(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].closeDamageColliderBuffer); 
        meleeDamageCollider.DisableDamageCollider(); //DISABLE DAMAGE COLLIDER
        attackMomentumActivated = false; //DISABLE ATTACK MOMENTUM
    }

    public void AdjustAttackMomentum()
    {
        if (attackMomentumActivated)
        {
            playerManager.rb.AddForce((playerManager.transform.up * playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].attackMomentum));
        }
    }
    
    private void PlayMeleeVFX()
    {
        GameObject meleeMotionVFXGO = Instantiate(meleeMotionVFX, meleeMotionVFXTransform.position, transform.rotation);
        meleeMotionVFXGO.transform.parent = transform;
        meleeMotionVFXGO.GetComponentInChildren<Animator>().Play(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].attackAnimations[0]);
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