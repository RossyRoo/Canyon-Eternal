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
        activeWeaponIcon.sprite = playerInventory.activeWeapon.dataIcon;
        activeOffhandIcon.sprite = playerInventory.activeOffhandWeapon.dataIcon;
        activeSpellIcon.sprite = playerInventory.activeSpell.dataIcon;
        activeItemIcon.sprite = playerInventory.activeConsumable.dataIcon;

    }
}
