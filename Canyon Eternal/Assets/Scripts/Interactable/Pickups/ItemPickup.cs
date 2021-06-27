using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item thisItem;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        AddToInventory(playerManager);
    }

    private void AddToInventory(PlayerManager playerManager)
    {
        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();

        if(thisItem.GetType()==typeof(Treasure))
        {
            playerInventory.treasureInventory.Add((Treasure)thisItem);
            Debug.Log("Added treasure to inventory");
        }
        else if(thisItem.GetType() == typeof(Usable))
        {
            playerInventory.usableInventory.Add((Usable)thisItem);
        }
        else if(thisItem.GetType() == typeof(Key))
        {
            playerInventory.keyInventory.Add((Key)thisItem);
        }
        else if(thisItem.GetType() == typeof(MeleeWeapon))
        {
            MeleeWeapon thisMelee = (MeleeWeapon)thisItem;

            if(thisMelee.isThrust)
            {
                playerInventory.thrustWeaponsInventory.Add(thisMelee);
            }
            else if(thisMelee.isSlash)
            {
                playerInventory.slashWeaponsInventory.Add(thisMelee);
            }
            else
            {
                playerInventory.strikeWeaponsInventory.Add(thisMelee);
            }
        }
        else if(thisItem.GetType() == typeof(Spell))
        {
            Spell thisSpell = (Spell)thisItem;
        }
    }
}
