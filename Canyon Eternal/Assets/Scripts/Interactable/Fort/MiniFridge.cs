using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFridge : Interactable
{
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        RefillLunchbox(playerManager);
    }

    public void RefillLunchbox(PlayerManager playerManager)
    {
        PlayerStats playerStats = playerManager.GetComponent<PlayerStats>();
        playerStats.currentLunchBoxCapacity = playerStats.maxLunchBoxCapacity;
        playerStats.lunchboxMeter.SetCurrentLunchBox(playerStats.currentLunchBoxCapacity);
    }
}
