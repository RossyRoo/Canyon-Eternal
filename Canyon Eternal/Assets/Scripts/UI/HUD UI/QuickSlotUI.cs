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
        activeWeaponIcon.sprite = playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber].dataIcon;
        activeOffhandIcon.sprite = playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber].dataIcon;
        activeSpellIcon.sprite = playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].dataIcon;
        activeItemIcon.sprite = playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber].dataIcon;

    }
}
