using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerStats playerStats;

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
    public bool canContinueCombo = false;
    public bool comboFlag;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
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
        currentMeleeModel = meleeModelPrefab;
        meleeDamageCollider = currentMeleeModel.GetComponentInChildren<DamageCollider>();
        meleeModelPrefab.SetActive(false);
    }

    public void HandleMeleeAttack()
    {
        if (playerStats.currentStamina < activeMeleeCard.staminaCost)
            return;

        if (comboNumber==0)
        {
            playerAnimatorHandler.PlayTargetAnimation(activeMeleeCard.attackAnimation, true);
            playerStats.LoseStamina(activeMeleeCard.staminaCost);
            currentMeleeModel.SetActive(true);
        }
    }

    public void SetDamage()
    {

    }
}
