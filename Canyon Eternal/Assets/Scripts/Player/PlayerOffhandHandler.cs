using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffhandHandler : MonoBehaviour
{
    PlayerInventory playerInventory;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;


    GameObject currentOffhandModel;
    public Transform offhandParentOverride;


    public AudioClip blockMissedSFX;

    float blockDuration = 0.35f;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();

        if(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] == null)
        {
            if(playerInventory.offhandWeaponInventory.Count > 0)
            {
                playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] = playerInventory.offhandWeaponInventory[0];
            }
        }
        else
        {
            LoadOffhandModel();
        }
    }


    public void LoadOffhandModel()
    {
        GameObject offhandModelPrefab = Instantiate(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].modelPrefab) as GameObject;

        if (offhandModelPrefab != null)
        {
            if (offhandParentOverride != null)
            {
                offhandModelPrefab.transform.SetParent(offhandParentOverride);
                offhandModelPrefab.transform.localScale = playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].modelPrefab.transform.localScale;
            }

        }

        currentOffhandModel = offhandModelPrefab;
    }

    public void DestroyOffhandModel()
    {
        Destroy(currentOffhandModel);
        currentOffhandModel = null;
    }

    public void HandleStartBlock()
    {
        BlockCollider blockCollider = currentOffhandModel.GetComponent<BlockCollider>();

        playerManager.isBlocking = true;

        playerAnimatorHandler.PlayTargetAnimation(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].attackAnimations[1], true);

        currentOffhandModel.GetComponent<Collider2D>().enabled = true;
        blockCollider.blockMode = true;
    }

    public void HandleStopBlock()
    {
        BlockCollider blockCollider = currentOffhandModel.GetComponent<BlockCollider>();

        playerManager.isBlocking = false;

        playerAnimatorHandler.PlayTargetAnimation(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].attackAnimations[2], true);

        blockCollider.blockMode = false;
        currentOffhandModel.GetComponent<Collider2D>().enabled = false;
    }

    public IEnumerator HandleParrying()
    {
        BlockCollider blockCollider = currentOffhandModel.GetComponent<BlockCollider>();

        playerManager.isParrying = true;

        playerStats.LoseStamina(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].staminaCost);
        playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames);

        playerAnimatorHandler.PlayTargetAnimation(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].attackAnimations[0], true);
        SFXPlayer.Instance.PlaySFXAudioClip(blockMissedSFX, 0.3f, 0.2f);

        currentOffhandModel.GetComponent<Collider2D>().enabled = true;
        blockCollider.parryMode = true;

        yield return new WaitForSeconds(blockDuration);

        currentOffhandModel.GetComponent<Collider2D>().enabled = false;
        playerManager.isParrying = false;
        blockCollider.parryMode = false;
    }

}
