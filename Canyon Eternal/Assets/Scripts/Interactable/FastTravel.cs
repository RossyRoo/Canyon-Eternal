using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravel : Interactable
{
    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        OpenFastTravelMenu();
    }

    public void OpenFastTravelMenu()
    {
        Debug.Log("Opening fast travel menu");
    }
}
 