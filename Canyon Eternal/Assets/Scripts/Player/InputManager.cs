﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerSpellHandler playerSpellHandler;
    PlayerOffhandHandler playerOffhandHandler;
    PlayerParticleHandler playerParticleHandler;
    ConsumableHandler consumableHandler;
    GameMenuUI gameMenuUI;

    //INPUT DECLARATIONS
    public Vector2 moveInput;
    public Vector2 mouseInput;

    public bool melee_Input;
    public bool startBlock_Input;
    public bool stopBlock_Input;
    public bool chargeSpell_Input;
    public bool castSpell_Input;
    public bool dash_Input;
    public bool parry_Input;
    public bool heal_Input;
    public bool item_Input;
    public bool interact_Input;
    public bool openMenu_Input;
    public bool cycleMenuLeft_Input;
    public bool cycleMenuRight_Input;
    public bool cycleSubmenuLeft_Input;
    public bool cycleSubmenuRight_Input;

    //VARIABLES
    
    public float rotateSpeed = 10f;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerSpellHandler = GetComponent<PlayerSpellHandler>();
        playerOffhandHandler = GetComponent<PlayerOffhandHandler>();
        consumableHandler = FindObjectOfType<ConsumableHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();

        gameMenuUI = FindObjectOfType<GameMenuUI>();
    }

    private void OnEnable()
    {
        if(playerControls==null)
        {
            playerControls = new PlayerControls();

            //ROTATION
            playerControls.PlayerMovement.Rotate.performed += i => mouseInput = i.ReadValue<Vector2>();

            //MOVEMENT
            playerControls.PlayerMovement.Move.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Move.canceled += i => moveInput = Vector2.zero;

            //MELEE
            playerControls.PlayerActions.Melee.performed += i => melee_Input = true;

            //PARRY
            playerControls.PlayerActions.Parry.performed += i => parry_Input = true;

            //BLOCK
            playerControls.PlayerActions.Block.performed += i => startBlock_Input = true;
            playerControls.PlayerActions.Block.canceled += i => stopBlock_Input = true;

            //SPELL
            playerControls.PlayerActions.Spell.performed += i => chargeSpell_Input = true;
            playerControls.PlayerActions.Spell.canceled += i => castSpell_Input = true;

            //DASH
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;

            //HEAL
            playerControls.PlayerActions.Heal.performed += i => heal_Input = true;

            //ITEM
            playerControls.PlayerActions.Item.performed += i => item_Input = true;

            //INTERACT
            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;

            //UI
            playerControls.UI.OpenMenu.performed += i => openMenu_Input = true;
            playerControls.UI.CycleMenuLeft.performed += i => cycleMenuLeft_Input = true;
            playerControls.UI.CycleMenuRight.performed += i => cycleMenuRight_Input = true;
            playerControls.UI.CycleSubmenuLeft.performed += i => cycleSubmenuLeft_Input = true;
            playerControls.UI.CycleSubmenuRight.performed += i => cycleSubmenuRight_Input = true;

            //DIALOGUE SYSTEM
            InputDeviceManager.RegisterInputAction("Submit", playerControls.PlayerActions.Interact);
            InputDeviceManager.RegisterInputAction("Cancel", playerControls.PlayerActions.Dash);
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMouseInput();
        HandleMeleeInput();
        HandleStartBlockInput();
        HandleStopBlockInput();
        HandleChargeSpellInput();
        HandleCastSpellInput();
        HandleDashInput();
        HandleParryInput();
        HandleHealInput();
        HandleItemInput();
        HandleMenuInput();
        HandleCloseInput();

        if(gameMenuUI.gameMenuIsOpen)
        {
            HandleCycleMenuInput();
            HandleCycleSubmenuInput();
        }
    }

    private void HandleMouseInput()
    {
        if(playerManager.isFalling
            || playerManager.isDead
            || playerManager.isInteractingWithUI)
        {
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseInput);
        Vector2 relativePosition = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        float angle = (Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg) - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
            Time.deltaTime * rotateSpeed);
    }

    #region Handle Combat Input
    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            if (playerManager.isInteracting
                || playerManager.isInteractingWithUI
                || playerManager.isFalling
                || playerStats.currentStamina < playerInventory.activeWeapon.staminaCost
                || playerMeleeHandler.currentAttackCooldownTime > 0)
                return;

            StartCoroutine(playerMeleeHandler.HandleMeleeAttack());
        }
    }

    private void HandleChargeSpellInput()
    {
        if(chargeSpell_Input)
        {
            if (playerManager.isInteracting || playerManager.isConversing)
                return;

            playerSpellHandler.ChargeSpell();
        }
    }

    private void HandleCastSpellInput()
    {
        if(castSpell_Input)
        {
            if(playerManager.isCastingSpell)
            {
                playerSpellHandler.HandleAllSpellCasting();
            }
            else
            {
                if(playerManager.isChargingSpell)
                {
                    playerSpellHandler.CancelSpell();
                }
            }
        }
    }

    private void HandleDashInput()
    {
        if(dash_Input)
        {
            if (playerManager.isInteracting
                || playerManager.isInteractingWithUI
                || playerStats.currentStamina < 1 || playerManager.isFalling)
                return;

            playerManager.isDashing = true;
        }
    }

    private void HandleParryInput()
    {
        if(parry_Input)
        {
            if (playerStats.currentStamina < playerInventory.activeOffhandWeapon.staminaCost || playerManager.isInteracting || playerManager.isInteractingWithUI)
                return;

            StartCoroutine(playerOffhandHandler.HandleParrying());
        }
    }

    private void HandleStartBlockInput()
    {
        if(startBlock_Input)
        {
            if (playerStats.currentStamina < playerInventory.activeOffhandWeapon.staminaCost || playerManager.isInteracting || playerManager.isInteractingWithUI)
                return;

            playerOffhandHandler.HandleStartBlock();
        }
    }

    private void HandleStopBlockInput()
    {
        if(stopBlock_Input && playerManager.isBlocking)
        {
            playerOffhandHandler.HandleStopBlock();
        }
    }

    private void HandleHealInput()
    {
        if(heal_Input)
        {
            if (playerManager.isDead || playerManager.isConversing)
                return;

            if (playerStats.currentLunchBoxCapacity > 0 && playerStats.currentHealth < playerStats.characterData.startingMaxHealth)
            {
                playerParticleHandler.SpawnHealVFX();
                playerStats.RecoverHealth(playerStats.characterData.healAmount, false, false);
            }
        }

    }

    private void HandleItemInput()
    {
        if (item_Input)
        {
            if (playerManager.isDead || playerManager.isConversing ||
                playerInventory.activeConsumable == null || !playerInventory.itemInventory.Contains(playerInventory.activeConsumable))
                return;

            consumableHandler.HandlePlayerConsumable(playerInventory.activeConsumable, playerStats);
        }
    }

    #endregion

    private void HandleMenuInput()
    {
        if (playerManager.isInteracting && !playerManager.isInteractingWithUI)
            return;

        if(openMenu_Input)
        {
            gameMenuUI.ReverseGameMenuUI(false);
        }
    }

    private void HandleCycleMenuInput()
    {
        if (cycleMenuRight_Input)
        {
            gameMenuUI.CycleMenuRight();
        }

        if (cycleMenuLeft_Input)
        {
            gameMenuUI.CycleMenuLeft();
        }
    }

    private void HandleCycleSubmenuInput()
    {
        if(cycleSubmenuLeft_Input)
        {
            gameMenuUI.CycleSubmenuLeft();
        }

        if(cycleSubmenuRight_Input)
        {
            gameMenuUI.CycleSubmenuRight();
        }
    }

    private void HandleCloseInput()
    {
        if(dash_Input)
        {
            if(!playerManager.isInteractingWithUI)
            {
                return;
            }
        }
    }
}
