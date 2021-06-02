using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    public BlockCollider blockCollider;

    public GameObject shieldModel;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        blockCollider = GetComponentInChildren<BlockCollider>();
    }

    public void HandleBlocking()
    {
        playerManager.isBlocking = true;

        if (playerManager.isInteracting)
        {

        }
        else
        {
            playerAnimatorHandler.PlayTargetAnimation("Block", true);
            Invoke("SpawnShieldModel", 0.1f);
        }

    }

    private void SpawnShieldModel()
    {
        shieldModel.SetActive(true);
    }
}
