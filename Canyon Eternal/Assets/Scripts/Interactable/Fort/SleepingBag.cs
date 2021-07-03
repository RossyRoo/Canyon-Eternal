using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingBag : Interactable
{

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        FullyRecover(playerManager);
        //Play Full Heal Anim and Particles
    }

    private void FullyRecover(PlayerManager playerManager)
    {
        PlayerStats playerStats = playerManager.GetComponent<PlayerStats>();
        playerStats.RecoverHealth(8, true);
    }
}
