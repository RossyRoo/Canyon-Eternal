using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mirror : Interactable
{/*
    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerInventory playerInventory;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerSpellHandler playerSpellHandler;

    public GameObject mirrorUIGO;

    public bool mirrorIsOpen;

    public int thrustIndex;
    public int slashIndex;
    public int strikeIndex;
    public int projectileIndex;
    public int aOEIndex;
    public int buffIndex;

    public TextMeshProUGUI currentThrustText;
    public TextMeshProUGUI currentSlashText;
    public TextMeshProUGUI currentStrikeText;
    public TextMeshProUGUI currentProjectileText;
    public TextMeshProUGUI currentAOEText;
    public TextMeshProUGUI currentBuffText;



    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        ReverseMirrorUI();
    }

    public void ReverseMirrorUI()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerAnimatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

        if (mirrorUIGO.activeInHierarchy)
        {
            mirrorIsOpen = false;
            playerManager.isInteractingWithUI = false;
            playerAnimatorHandler.animator.SetBool("isInteracting", false);
            mirrorUIGO.SetActive(false);
        }
        else
        {
            mirrorIsOpen = true;
            playerManager.isInteractingWithUI = true;
            playerAnimatorHandler.animator.SetBool("isInteracting", true);
            mirrorUIGO.SetActive(true);

            DisplayCurrentInventoryIndices();
        }
    }

    public void EquipMelee(int meleeType)
    {
        playerMeleeHandler = playerManager.GetComponent<PlayerMeleeHandler>();

        if (meleeType == 0)
        {
            playerMeleeHandler.activeMeleeWeapon = playerInventory.weaponsInventory[thrustIndex];
        }
        else if(meleeType == 1)
        {
            playerMeleeHandler.activeMeleeWeapon = playerInventory.slashWeaponsInventory[slashIndex];
        }
        else if(meleeType == 2)
        {
            playerMeleeHandler.activeMeleeWeapon = playerInventory.strikeWeaponsInventory[strikeIndex];
        }

        playerMeleeHandler.DestroyMelee();
        playerMeleeHandler.SetParentOverride();
        playerMeleeHandler.LoadMelee();
    }

    public void EquipSpell(int spellType)
    {
        playerSpellHandler = playerManager.GetComponent<PlayerSpellHandler>();

        if (spellType == 0)
        {
            playerSpellHandler.activeSpell = playerInventory.projectileSpellsInventory[projectileIndex];
        }
        else if (spellType == 1)
        {
            playerSpellHandler.activeSpell = playerInventory.aOESpellsInventory[aOEIndex];
        }
        else if (spellType == 2)
        {
            playerSpellHandler.activeSpell = playerInventory.buffSpellsInventory[buffIndex];
        }
    }

    public void CycleThrustIndex()
    {
        if(playerInventory.weaponsInventory.Count < 2)
        {
            return;
        }

        if(thrustIndex != playerInventory.weaponsInventory.Count - 1)
        {
            thrustIndex++;
        }
        else
        {
            thrustIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void CycleSlashIndex()
    {
        if (playerInventory.slashWeaponsInventory.Count < 2)
        {
            return;
        }

        if (slashIndex != playerInventory.slashWeaponsInventory.Count - 1)
        {
            slashIndex++;
        }
        else
        {
            slashIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void CycleStrikeIndex()
    {
        if (playerInventory.strikeWeaponsInventory.Count < 2)
        {
            return;
        }

        if (strikeIndex != playerInventory.strikeWeaponsInventory.Count - 1)
        {
            strikeIndex++;
        }
        else
        {
            strikeIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void CycleProjectileIndex()
    {
        if (playerInventory.projectileSpellsInventory.Count < 2)
        {
            return;
        }

        if (projectileIndex != playerInventory.projectileSpellsInventory.Count - 1)
        {
            projectileIndex++;
        }
        else
        {
            projectileIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void CycleAOEIndex()
    {
        if (playerInventory.aOESpellsInventory.Count < 2)
        {
            return;
        }

        if (aOEIndex != playerInventory.aOESpellsInventory.Count - 1)
        {
            aOEIndex++;
        }
        else
        {
            aOEIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void CycleBuffIndex()
    {
        if (playerInventory.buffSpellsInventory.Count < 2)
        {
            return;
        }

        if (buffIndex != playerInventory.buffSpellsInventory.Count - 1)
        {
            buffIndex++;
        }
        else
        {
            buffIndex = 0;
        }
        DisplayCurrentInventoryIndices();
    }

    public void DisplayCurrentInventoryIndices()
    {
        currentThrustText.text = playerInventory.weaponsInventory[thrustIndex].dataName;
        currentSlashText.text = playerInventory.slashWeaponsInventory[slashIndex].dataName;
        currentStrikeText.text = playerInventory.strikeWeaponsInventory[strikeIndex].dataName;
        currentProjectileText.text = playerInventory.projectileSpellsInventory[projectileIndex].dataName;
        currentAOEText.text = playerInventory.aOESpellsInventory[aOEIndex].dataName;
        currentBuffText.text = playerInventory.buffSpellsInventory[buffIndex].dataName;
    }
*/
}
