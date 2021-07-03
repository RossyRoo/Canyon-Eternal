using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoOpportunity : Interactable
{
    public Photo thisPhoto;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        TakePhoto(playerManager);
    }

    public void TakePhoto(PlayerManager playerManager)
    {
        playerManager.GetComponent<PlayerInventory>().photoInventory.Add(thisPhoto);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
