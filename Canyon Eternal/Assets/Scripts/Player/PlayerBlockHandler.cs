using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;
    public BlockCollider blockCollider;

    public GameObject shieldModel;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        blockCollider = GetComponentInChildren<BlockCollider>();
    }

    public IEnumerator HandleBlocking()
    {
        playerManager.isBlocking = true;

        if (playerManager.isInteracting)
        {
            yield break;
        }
        else
        {
            playerAnimatorHandler.PlayTargetAnimation("Block", true);

            yield return new WaitForSeconds(0.1f);

            shieldModel.SetActive(true);
        }

        playerStats.LoseStamina(1);
    }

}
