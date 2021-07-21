using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : Interactable
{
    PlayerProgression playerProgression;

    float collectItemBuffer = 1.5f;
    public DataObject thisData;
    public Sprite openContainerSprite;
    public int chestID;

    private void Start()
    {
        playerProgression = FindObjectOfType<PlayerProgression>();

        if (playerProgression.collectedChestIDs.Contains(chestID))
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

        if (!playerProgression.collectedChestIDs.Contains(chestID))
        {
            playerProgression.collectedChestIDs.Add(chestID);
        }

        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();

        if(thisData.GetType() == typeof(Item))
        {
            playerInventory.itemInventory.Add((Item)thisData);
        }
        else if(thisData.GetType() == typeof(MeleeWeapon))
        {
            playerInventory.weaponsInventory.Add((MeleeWeapon)thisData);
        }
        else if(thisData.GetType() == typeof(Spell))
        {
            playerInventory.spellsInventory.Add((Spell)thisData);
        }

        playerManager.isInteractingWithUI = true;

        playerManager.itemPopupGO.GetComponentInChildren<TextMeshProUGUI>().text = thisData.dataName;
        playerManager.itemPopupGO.GetComponentInChildren<Image>().sprite = thisData.dataIcon;
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
