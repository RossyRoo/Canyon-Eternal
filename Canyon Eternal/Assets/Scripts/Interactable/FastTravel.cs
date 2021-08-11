using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravel : Interactable
{
    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        TriggerOpenFastTravelMenu();
    }

    public void TriggerOpenFastTravelMenu()
    {
        FindObjectOfType<FastTravelUI>().OpenFastTravel();
    }
}
 