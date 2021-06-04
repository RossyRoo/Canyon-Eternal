using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBook : Interactable
{
    PlayerMeleeHandler playerMeleeHandler;
    PlayerInventory playerInventory;

    public GameObject cardbookUI;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        DisplayEquipmentChangingUI();
    }

    private void DisplayEquipmentChangingUI()
    {
        cardbookUI.SetActive(true);
    }

    public void ChangeMelee(int meleeType)
    {
        PlayerMeleeHandler playerMeleeHandler = FindObjectOfType<PlayerMeleeHandler>();
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

        if (meleeType == 0)
        {
            playerMeleeHandler.activeMeleeCard = playerInventory.heldThrustCard;
        }
        else if(meleeType == 1)
        {
            playerMeleeHandler.activeMeleeCard = playerInventory.heldSlashCard;
        }
        else if(meleeType == 2)
        {
            playerMeleeHandler.activeMeleeCard = playerInventory.heldStrikeCard;
        }

        playerMeleeHandler.DestroyMelee();
        playerMeleeHandler.SetParentOverride();
        playerMeleeHandler.LoadMelee();
    }
}
