using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerSpellHandler playerSpellHandler;
    PlayerBlockHandler playerBlockHandler;

    //INPUT DECLARATIONS
    public Vector2 moveInput;

    public bool melee_Input;
    public bool chargeSpell_Input;
    public bool castSpell_Input;
    public bool dash_Input;
    public bool block_Input;
    public bool heal_Input;
    public bool interact_Input;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerSpellHandler = GetComponent<PlayerSpellHandler>();
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

            //MELEE
            playerControls.PlayerActions.Melee.performed += i => melee_Input = true;

            //SPELL
            playerControls.PlayerActions.Spell.performed += i => chargeSpell_Input = true;
            playerControls.PlayerActions.Spell.canceled += i => castSpell_Input = true;

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
        HandleChargeSpellInput();
        HandleCastSpellInput();
        HandleDashInput();
        HandleBlockInput();
        HandleHealInput();
    }
    #region Handle Combat Input
    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            if(playerManager.isAttacking)
            {
                playerMeleeHandler.HandleComboAttempt();
            }

            if (playerManager.isInteracting || playerStats.currentStamina < playerMeleeHandler.activeMeleeCard.staminaCost)
                return;

            StartCoroutine(playerMeleeHandler.BeginNewAttackChain());
        }
    }

    private void HandleChargeSpellInput()
    {
        if(chargeSpell_Input)
        {
            if (playerManager.isInteracting)
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
            if (playerManager.isInteracting || playerStats.currentStamina < 1)
                return;

            playerManager.isDashing = true;
        }
    }

    private void HandleBlockInput()
    {
        if(block_Input)
        {
            if (playerStats.currentStamina < 1)
                return;

            StartCoroutine(playerBlockHandler.HandleBlocking());
        }
    }

    private void HandleHealInput()
    {
        if(heal_Input)
        {
            if (playerManager.isDead)
                return;

            if (playerStats.currentLunchBoxCapacity > 0 && playerStats.characterData.currentHealth < playerStats.characterData.maxHealth)
            {
                playerStats.RecoverHealth(playerStats.characterData.healAmount, false);
            }
        }

    }
    #endregion
}
