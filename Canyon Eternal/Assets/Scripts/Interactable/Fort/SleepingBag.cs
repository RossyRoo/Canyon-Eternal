using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingBag : Interactable
{

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        FullyRecover(playerManager);
        RespawnEnemies();
    }

    private void FullyRecover(PlayerManager playerManager)
    {
        PlayerStats playerStats = playerManager.GetComponent<PlayerStats>();
        playerStats.RecoverHealth(8, true);
        Debug.Log("Healed");
    }

    private void RespawnEnemies()
    {

    }
}
