using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Image activeWeaponIcon;
    public Image activeOffhandIcon;
    public Image activeSpellIcon;
    public Image activeItemIcon;

    public void UpdateQuickSlotIcons(PlayerInventory playerInventory)
    {
        if(playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] != null)
        {
            activeWeaponIcon.enabled = true;
            activeWeaponIcon.sprite = playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].dataIcon;
        }
        else
        {
            activeWeaponIcon.sprite = null;
            activeWeaponIcon.enabled = true;
        }

        if(playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] != null)
        {
            activeOffhandIcon.enabled = true;
            activeOffhandIcon.sprite = playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].dataIcon;
        }
        else
        {
            activeOffhandIcon.sprite = null;
            activeOffhandIcon.enabled = false;
        }

        if(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] != null)
        {
            activeSpellIcon.enabled = true;
            activeSpellIcon.sprite = playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].dataIcon;
        }
        else
        {
            activeSpellIcon.enabled = false;
            activeSpellIcon.sprite = null;
        }

        if(playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber] != null)
        {
            activeItemIcon.enabled = true;
            activeItemIcon.sprite = playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber].dataIcon;

        }
        else
        {
            activeItemIcon.enabled = false;
            activeItemIcon.sprite = null;
        }
    }
}
