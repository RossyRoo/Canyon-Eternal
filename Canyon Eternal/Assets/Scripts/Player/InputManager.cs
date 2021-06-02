﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerBlockHandler playerBlockHandler;

    //INPUT DECLARATIONS
    public Vector2 moveInput;

    public bool melee_Input;
    public bool dash_Input;
    public bool block_Input;
    public bool heal_Input;
    public bool interact_Input;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerBlockHandler = GetComponent<PlayerBlockHandler>();
    }

    private void OnEnable()
    {
        if(playerControls==null)
        {
            playerControls = new PlayerControls();

            //MOVEMENT
            playerControls.PlayerMovement.Move.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Move.canceled += i => moveInput = Vector2.zero;

            //ATTACKING
            playerControls.PlayerActions.Melee.performed += i => melee_Input = true;

            //DASH
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;

            //BLOCK
            playerControls.PlayerActions.Block.performed += i => block_Input = true;

            //HEAL
            playerControls.PlayerActions.Heal.performed += i => heal_Input = true;

            //INTERACT
            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMeleeInput();
        HandleDashInput();
        HandleBlockInput();
        HandleHealInput();
    }

    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            if(playerManager.isDashing)
            {
                return;
            }

            playerMeleeHandler.HandleComboAttempt();

            if (playerManager.isInteracting)
                return;

            playerMeleeHandler.BeginNewAttackChain();
        }
    }

    private void HandleDashInput()
    {
        if(dash_Input)
        {
            if (playerManager.isInteracting)
                return;

            playerManager.isDashing = true;
        }
    }

    private void HandleBlockInput()
    {
        if(block_Input)
        {
            playerBlockHandler.HandleBlocking();
        }
    }

    private void HandleHealInput()
    {
        if (playerStats.currentLunchBoxCapacity > 0
            && playerStats.currentHealth < playerStats.maxHealth)
        {
            if (heal_Input)
            {
                playerStats.RecoverHealth(playerStats.healAmount, false);
            }
        }
    }

}
