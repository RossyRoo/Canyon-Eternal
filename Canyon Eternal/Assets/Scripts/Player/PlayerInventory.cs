using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    FragmentCounterUI fragmentCounterUI;
    QuickSlotUI quickSlotUI;

    public int fragmentInventory;

    [Header("Melee Inventory")]
    public List<MeleeWeapon> weaponsInventory;
    public MeleeWeapon[] weaponSlots;
    public int activeWeaponSlotNumber;

    [Header("Offhand Inventory")]
    public List<OffhandWeapon> offhandWeaponInventory;
    public OffhandWeapon[] offhandSlots;
    public int activeOffhandWeaponSlotNumber;

    [Header("Spell Inventory")]
    public List<Spell> spellsInventory;
    public Spell[] spellSlots;
    public int activeSpellSlotNumber;

    [Header("Item Inventory")]
    public List<Item> itemInventory;
    public Consumable[] consumableSlots;
    public int activeConsumableSlotNumber;

    [Header("Gear Inventory")]
    public Gear activeGear;
    public List<Gear> gearInventory;

    [Header("Photo Inventory")]
    public List<Photo> photoInventory;

    [Header("Artifact Entry Collection")]
    public List<DataObject> artifactInventory;


    private void Awake()
    {
        fragmentCounterUI = FindObjectOfType<FragmentCounterUI>();
        quickSlotUI = FindObjectOfType<QuickSlotUI>();

        quickSlotUI.UpdateQuickSlotIcons(this);
    }

    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;

        if(adjustment >= 0)
        {
            StartCoroutine(fragmentCounterUI.IncreaseFragmentCountUI(adjustment));
        }
        else
        {
            StartCoroutine(fragmentCounterUI.DecreaseFragmentCountUI(adjustment));
        }
    }
}
