﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerParticleHandler playerParticleHandler;

    //INPUT DECLARATIONS
    public Vector2 moveInput;
    public Vector2 lastMoveInput;

    public bool melee_Input;
    public bool dash_Input;
    public bool guard_Input;
    public bool heal_Input;
    public bool interact_Input;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
    }

    private void Start()
    {
        lastMoveInput = new Vector2(0, -1);
    }

    private void OnEnable()
    {
        if(playerControls==null)
        {
            playerControls = new PlayerControls();

            //MOVEMENT
            playerControls.PlayerMovement.Move.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Move.canceled += i => lastMoveInput = moveInput;
            playerControls.PlayerMovement.Move.canceled += i => moveInput = Vector2.zero;

            //ATTACKING
            playerControls.PlayerActions.Melee.performed += i => melee_Input = true;

            //DASH
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;

            //GUARD
            playerControls.PlayerActions.Guard.performed += i => guard_Input = true;

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
        HandleGuardInput();
        HandleHealInput();
    }

    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            playerParticleHandler.ResetComboStarMaterial();

            if(playerMeleeHandler.canContinueCombo)
            {
                playerMeleeHandler.comboWasHit = true;
                playerMeleeHandler.PlayComboHitFX();
                playerParticleHandler.ChangeComboStarMat();
            }
            else if(!playerMeleeHandler.canContinueCombo && playerManager.isAttacking)
            {
                playerMeleeHandler.comboWasMissed = true;
                playerParticleHandler.ChangeComboStarMat();
            }

            if (playerManager.isInteracting)
                return;

            if(playerMeleeHandler.comboWasMissed)
            {
                playerMeleeHandler.comboWasMissed = false;
            }

            playerMeleeHandler.BeginAttackChain();
        }
    }

    private void HandleDashInput()
    {
        if(dash_Input)
        {
            if (playerManager.isInteracting)
                return;

            playerManager.dashFlag = true;
        }
    }

    private void HandleGuardInput()
    {
        if(guard_Input)
        {
            Debug.Log("Guard");
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
