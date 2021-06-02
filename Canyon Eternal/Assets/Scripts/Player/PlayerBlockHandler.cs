﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;

    public GameObject shieldModel;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
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

    public void ActivateBlockCollider()
    {
        //A script on the shield will activate a collider whose job is to detect an enemy that is attacking
        //If the enemy was not attacking, the block uses stamina and does not stun the enemy
        //If the enemy was attacking, the block does not use stamina, stuns the enemy, and turns off that enemies damage collider
        //During the stunned state, an enemy plays a brief animation in which they are vulnerable and then goes back to a combat state
    }
}
