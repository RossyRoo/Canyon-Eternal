using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerStats playerStats;

    [Header("Weapon Loading")]
    public MeleeCard activeMeleeCard;
    public GameObject currentMeleeModel;
    public Transform parentOverride;
    public DamageCollider meleeDamageCollider;


    private void Awake()
    {
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        LoadMelee();
    }

    public void UnloadMelee()
    {
        if (currentMeleeModel != null)
        {
            currentMeleeModel.SetActive(false);
        }
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
            //meleeModelPrefab.transform.localScale = Vector3.one;
        }
        currentMeleeModel = meleeModelPrefab;
        meleeDamageCollider = currentMeleeModel.GetComponentInChildren<DamageCollider>();
        meleeModelPrefab.SetActive(false);
    }

    public void HandleMeleeAttack()
    {
        if (playerStats.currentStamina < activeMeleeCard.staminaCost)
            return;

        currentMeleeModel.SetActive(true);
        playerAnimatorHandler.PlayTargetAnimation(activeMeleeCard.attackAnimation, true);
        playerStats.LoseStamina(activeMeleeCard.staminaCost);
    }
}
