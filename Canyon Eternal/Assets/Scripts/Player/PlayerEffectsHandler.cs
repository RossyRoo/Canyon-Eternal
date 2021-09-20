using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour
{
    public void UseConsumable(Consumable consumable)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        SFXPlayer.Instance.PlaySFXAudioClip(consumable.consumeSFX);
        Debug.Log("USING AN ITEM");

        if(consumable.isPermanentUpgrade) // PERM UPGRADES
        {
            playerStats.characterData.startingMaxHealth += consumable.healthAmount;
            playerStats.startingMaxStamina += consumable.staminaAmount;
            playerStats.SetStartingStats();
        }

        playerStats.RecoverHealth(consumable.healthAmount, false, true);
        playerStats.RecoverStamina(consumable.staminaAmount);


    }
}
