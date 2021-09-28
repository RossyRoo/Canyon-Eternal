using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHandler : MonoBehaviour
{
    public QuickSlotUI quickSlotUI;


    public void HandleEnemyConsumable(Consumable consumable, EnemyStats enemyStats)
    {
        Debug.Log(enemyStats.gameObject.name + " is using a " + consumable.dataName);
        ItemState itemState = enemyStats.GetComponentInChildren<ItemState>();

        SFXPlayer.Instance.PlaySFXAudioClip(consumable.consumeSFX);


        float amountHealed = enemyStats.currentHealth;

        enemyStats.currentHealth += (consumable.healthAmount * 100f);

        if (enemyStats.currentHealth > enemyStats.characterData.startingMaxHealth)
        {
            enemyStats.currentHealth = enemyStats.characterData.startingMaxHealth;
        }

        amountHealed = enemyStats.currentHealth - amountHealed;

        StartCoroutine(enemyStats.GetComponentInChildren<EnemyHealthBarUI>().SetHealthCoroutine(false, enemyStats.currentHealth, false, amountHealed));

        if (consumable.useVFX != null)
        {
            GameObject useVFX = Instantiate(consumable.useVFX, transform.position, Quaternion.identity);
            useVFX.transform.SetParent(FindObjectOfType<ObjectPool>().transform);
            Destroy(useVFX, 2f);
        }

        itemState.myConsumables.Remove(consumable);
    }

    public void HandlePlayerConsumable(Consumable consumable, PlayerStats playerStats)
    {
        PlayerInventory playerInventory = playerStats.GetComponent<PlayerInventory>();

        SFXPlayer.Instance.PlaySFXAudioClip(consumable.consumeSFX);

        if (consumable.isPermanentUpgrade) // permanent upgrades
        {
            playerStats.characterData.startingMaxHealth += consumable.healthAmount;
            playerStats.startingMaxStamina += consumable.staminaAmount;
            playerStats.SetStartingStats();
        }

        playerStats.RecoverHealth(consumable.healthAmount, false, true);
        playerStats.RecoverStamina(consumable.staminaAmount);

        //REMOVE FROM INVENTORY
        playerInventory.itemInventory.Remove(consumable);
        quickSlotUI.UpdateQuickSlotIcons(playerInventory);
        

    }

}
