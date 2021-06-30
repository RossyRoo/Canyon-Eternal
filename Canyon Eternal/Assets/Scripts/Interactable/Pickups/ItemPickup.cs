using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : Interactable
{
    PlayerProgression playerProgression;

    float collectItemBuffer = 1.5f;
    public Item thisItem;
    public Sprite openContainerSprite;
    public int chestID;

    private void Start()
    {
        playerProgression = FindObjectOfType<PlayerProgression>();

        if (playerProgression.completedChestIDs.Contains(chestID))
        {
            DeactivateItemPickup();
        }
    }

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        StartCoroutine(AddToInventory(playerManager));
    }

    private IEnumerator AddToInventory(PlayerManager playerManager)
    {
        DeactivateItemPickup();

        if (!playerProgression.completedChestIDs.Contains(chestID))
        {
            playerProgression.completedChestIDs.Add(chestID);
        }

        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();

        if(thisItem.GetType()==typeof(Treasure))
        {
            playerInventory.treasureInventory.Add((Treasure)thisItem);
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

            if(thisSpell.isProjectile)
            {
                playerInventory.projectileSpellsInventory.Add(thisSpell);
            }
            else if(thisSpell.isAOE)
            {
                playerInventory.aOESpellsInventory.Add(thisSpell);
            }
            else
            {
                playerInventory.buffSpellsInventory.Add(thisSpell);
            }
        }

        playerManager.isInteractingWithUI = true;

        playerManager.itemPopupGO.GetComponentInChildren<TextMeshProUGUI>().text = thisItem.itemName;
        playerManager.itemPopupGO.GetComponentInChildren<Image>().sprite = thisItem.itemIcon;
        playerManager.itemPopupGO.SetActive(true);

        yield return new WaitForSeconds(collectItemBuffer);

        playerManager.isInteractingWithUI = false;

        playerManager.itemPopupGO.SetActive(false);

    }

    public void DeactivateItemPickup()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = openContainerSprite;

    }
}
