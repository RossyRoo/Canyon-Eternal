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
    PlayerOffhandHandler playerOffhandHandler;
    PlayerParticleHandler playerParticleHandler;
    ConsumableHandler consumableHandler;
    GameMenuUI gameMenuUI;
    QuickSlotUI quickSlotUI;

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
    public bool dPadUp_Input;
    public bool dPadDown_Input;
    public bool dPadLeft_Input;
    public bool dPadRight_Input;


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
        quickSlotUI = FindObjectOfType<QuickSlotUI>();

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
            playerControls.UI.DPadUp.performed += i => dPadUp_Input = true;
            playerControls.UI.DPadDown.performed += i => dPadDown_Input = true;
            playerControls.UI.DPadLeft.performed += i => dPadLeft_Input = true;
            playerControls.UI.DPadRight.performed += i => dPadRight_Input = true;


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
        HandleDPadUp();
        HandleDPadDown();
        HandleDPadLeft();
        HandleDPadRight();

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
                || playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] == null
                || playerManager.isFalling
                || playerStats.currentStamina < playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].staminaCost
                || playerMeleeHandler.currentAttackCooldownTime > 0)
                return;

            StartCoroutine(playerMeleeHandler.HandleMeleeAttack());
        }
    }

    private void HandleChargeSpellInput()
    {
        if(chargeSpell_Input)
        {
            if (playerManager.isInteracting || playerManager.isConversing
                || playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] == null)
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
            if (playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] == null
                || playerStats.currentStamina < playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].staminaCost
                || playerManager.isInteracting || playerManager.isInteractingWithUI)
                return;

            StartCoroutine(playerOffhandHandler.HandleParrying());
        }
    }

    private void HandleStartBlockInput()
    {
        if(startBlock_Input)
        {
            if (playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] == null
                || playerStats.currentStamina < playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].staminaCost
                || playerManager.isInteracting || playerManager.isInteractingWithUI)
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
            if (playerManager.isDead || playerManager.isConversing
                || playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber] == null || !playerInventory.itemInventory.Contains(playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber]))
                return;

            consumableHandler.HandlePlayerConsumable(playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber], playerStats);
        }
    }

    #endregion

    #region Menus
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
    #endregion

    #region D-Pad

    private void HandleDPadUp()
    {
        if(dPadUp_Input)
        {
            if (playerInventory.activeSpellSlotNumber == 0)
            {
                playerInventory.activeSpellSlotNumber = 1;
            }
            else
            {
                playerInventory.activeSpellSlotNumber = 0;
            }
            quickSlotUI.UpdateQuickSlotIcons(playerInventory);
        }
    }

    private void HandleDPadDown()
    {
        if (dPadDown_Input)
        {
            if (playerInventory.activeConsumableSlotNumber == 0)
            {
                playerInventory.activeConsumableSlotNumber = 1;
            }
            else
            {
                playerInventory.activeConsumableSlotNumber = 0;
            }
            quickSlotUI.UpdateQuickSlotIcons(playerInventory);
        }
    }

    private void HandleDPadLeft()
    {
        if (dPadLeft_Input)
        {
            if (playerInventory.activeOffhandWeaponSlotNumber == 0)
            {
                playerInventory.activeOffhandWeaponSlotNumber = 1;
            }
            else
            {
                playerInventory.activeOffhandWeaponSlotNumber = 0;
            }
            if(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] != null)
            {
                playerOffhandHandler.LoadOffhandModel();
            }
            else
            {
                playerOffhandHandler.DestroyOffhandModel();
            }

            quickSlotUI.UpdateQuickSlotIcons(playerInventory);
        }
    }

    private void HandleDPadRight()
    {
        if (dPadRight_Input)
        {
            if(playerInventory.activeWeaponSlotNumber == 0)
            {
                playerInventory.activeWeaponSlotNumber = 1;
            }
            else
            {
                playerInventory.activeWeaponSlotNumber = 0;
            }
            if(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] != null)
            {
                playerMeleeHandler.SetMeleeParentOverride();
                playerMeleeHandler.LoadMeleeModel();
            }
            else
            {
                playerMeleeHandler.DestroyMeleeModel();
            }

            quickSlotUI.UpdateQuickSlotIcons(playerInventory);
        }
    }

    #endregion
}
