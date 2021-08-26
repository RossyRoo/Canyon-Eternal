using System.Collections;
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
    PlayerBlockHandler playerBlockHandler;
    GameMenuUI gameMenuUI;

    //INPUT DECLARATIONS
    public Vector2 moveInput;

    public bool melee_Input;
    public bool chargeSpell_Input;
    public bool castSpell_Input;
    public bool dash_Input;
    public bool block_Input;
    public bool heal_Input;
    public bool interact_Input;
    public bool openMenu_Input;
    public bool cycleMenuLeft_Input;
    public bool cycleMenuRight_Input;
    public bool cycleSubmenuLeft_Input;
    public bool cycleSubmenuRight_Input;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerSpellHandler = GetComponent<PlayerSpellHandler>();
        playerBlockHandler = GetComponent<PlayerBlockHandler>();

        gameMenuUI = FindObjectOfType<GameMenuUI>();
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
        HandleMeleeInput();
        HandleChargeSpellInput();
        HandleCastSpellInput();
        HandleDashInput();
        HandleBlockInput();
        HandleHealInput();
        HandleMenuInput();
        HandleCloseInput();

        if(gameMenuUI.gameMenuIsOpen)
        {
            HandleCycleMenuInput();
            HandleCycleSubmenuInput();
        }
    }
    #region Handle Combat Input
    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            if (playerManager.isInteracting
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
            if (playerManager.isInteracting || playerStats.currentStamina < 1 || playerManager.isFalling)
                return;

            playerManager.isDashing = true;
        }
    }

    private void HandleBlockInput()
    {
        if(block_Input)
        {
            if (playerStats.currentStamina < 1 || playerManager.isInteracting)
                return;

            StartCoroutine(playerBlockHandler.HandleBlocking());
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
                playerStats.RecoverHealth(playerStats.characterData.healAmount, false);
            }
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
