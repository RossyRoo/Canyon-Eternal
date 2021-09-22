using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    FragmentCounterUI fragmentCounterUI;
    QuickSlotUI quickSlotUI;

    public int fragmentInventory;

    [Header("Melee Inventory")]
    public MeleeWeapon activeWeapon;
    public List<MeleeWeapon> weaponsInventory;
    public MeleeWeapon[] quickWeaponSlots;

    [Header("Offhand Inventory")]
    public OffhandWeapon activeOffhandWeapon;
    public List<OffhandWeapon> offhandWeaponInventory;
    public OffhandWeapon[] quickOffhandSlots;

    [Header("Spell Inventory")]
    public Spell activeSpell;
    public List<Spell> spellsInventory;
    public Spell[] quickSpellSlots;

    [Header("Item Inventory")]
    public Consumable activeConsumable;
    public List<Item> itemInventory;
    public Item[] quickItemSlots;

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
