using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    BagUI bagUI;
    PlayerInventory playerInventory;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerOffhandHandler playerOffhandHandler;

    public Consumable testConsumable; //This is just so that I can tell the equipment what kind of menu to open for quick consumables
    public GameObject quickWeaponSlot1Button;
    public GameObject quickWeaponSlot2Button;
    public GameObject quickOffhandSlot1Button;
    public GameObject quickOffhandSlot2Button;
    public GameObject quickSpellSlot1Button;
    public GameObject quickSpellSlot2Button;
    public GameObject quickConsumableSlot1Button;
    public GameObject quickConsumableSlot2Button;
    public GameObject activeGearButton;
    public int quickSlotToChange;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        bagUI = GetComponent<BagUI>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMeleeHandler = FindObjectOfType<PlayerMeleeHandler>();
        playerOffhandHandler = FindObjectOfType<PlayerOffhandHandler>();
    }

    public void OpenEquipment()
    {
        gameMenuUI.submenuNameText.text = "Equipment";
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.infoPanel.SetActive(false);
        gameMenuUI.backButton.SetActive(false);
        gameMenuUI.equipmentUIGO.SetActive(true);

        SetUpWeaponSlots();
        SetUpOffhandSlots();
        SetUpSpellSlots();
        SetUpConsumableSlots();
        SetUpGearSlots();

    }

    #region Slots
    private void SetUpWeaponSlots()
    {
        if (playerInventory.weaponSlots[0] != null)
        {
            quickWeaponSlot1Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickWeaponSlot1Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.weaponSlots[0];
            quickWeaponSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.weaponSlots[0].dataIcon;
        }
        else
        {
            quickWeaponSlot1Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickWeaponSlot1Button.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[0];
        }

        if (playerInventory.weaponSlots[1] != null)
        {
            quickWeaponSlot2Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickWeaponSlot2Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.weaponSlots[1];
            quickWeaponSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.weaponSlots[1].dataIcon;
        }
        else
        {
            quickWeaponSlot2Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickWeaponSlot2Button.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[0];
        }
    }
    private void SetUpOffhandSlots()
    {
        if (playerInventory.offhandSlots[0] != null)
        {
            quickOffhandSlot1Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickOffhandSlot1Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.offhandSlots[0];
            quickOffhandSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.offhandSlots[0].dataIcon;
        }
        else
        {
            quickOffhandSlot1Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickOffhandSlot1Button.GetComponent<DataSlotUI>().slotData = playerInventory.offhandWeaponInventory[0];
        }

        if (playerInventory.offhandSlots[1] != null)
        {
            quickOffhandSlot2Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickOffhandSlot2Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.offhandSlots[1];
            quickOffhandSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.offhandSlots[1].dataIcon;
        }
        else
        {
            quickOffhandSlot2Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickOffhandSlot2Button.GetComponent<DataSlotUI>().slotData = playerInventory.offhandWeaponInventory[0];
        }
    }
    private void SetUpSpellSlots()
    {
        if (playerInventory.spellSlots[0] != null)
        {
            quickSpellSlot1Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickSpellSlot1Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.spellSlots[0];
            quickSpellSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.spellSlots[0].dataIcon;
        }
        else
        {
            quickSpellSlot1Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickSpellSlot1Button.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[0];
        }

        if (playerInventory.spellSlots[1] != null)
        {
            quickSpellSlot2Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickSpellSlot2Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.spellSlots[1];
            quickSpellSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.spellSlots[1].dataIcon;
        }
        else
        {
            quickSpellSlot2Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickSpellSlot2Button.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[0];
        }
    }
    private void SetUpConsumableSlots()
    {
        if (playerInventory.consumableSlots[0] != null)
        {
            quickConsumableSlot1Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickConsumableSlot1Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.consumableSlots[0];
            quickConsumableSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.consumableSlots[0].dataIcon;
        }
        else
        {
            quickConsumableSlot1Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickConsumableSlot1Button.GetComponent<DataSlotUI>().slotData = testConsumable;
        }

        if (playerInventory.consumableSlots[1] != null)
        {
            quickConsumableSlot2Button.GetComponent<DataSlotUI>().icon.enabled = true;
            quickConsumableSlot2Button.GetComponent<DataSlotUI>().slotData =
                playerInventory.consumableSlots[1];
            quickConsumableSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.consumableSlots[1].dataIcon;
        }
        else
        {
            quickConsumableSlot2Button.GetComponent<DataSlotUI>().icon.enabled = false;
            quickConsumableSlot2Button.GetComponent<DataSlotUI>().slotData = testConsumable;
        }
    }
    private void SetUpGearSlots()
    {
        if (playerInventory.activeGear != null)
        {
            activeGearButton.GetComponent<DataSlotUI>().icon.enabled = true;
            activeGearButton.GetComponent<DataSlotUI>().slotData =
                playerInventory.activeGear;
            activeGearButton.GetComponent<DataSlotUI>().icon.sprite =
                playerInventory.activeGear.dataIcon;
        }
        else
        {
            activeGearButton.GetComponent<DataSlotUI>().icon.enabled = false;
            activeGearButton.GetComponent<DataSlotUI>().slotData = playerInventory.gearInventory[0];
        }

    }
    #endregion

    #region Open Equipment Type Inventory
    public void SelectEquipmentToChange(DataSlotUI dataSlotUI)
    {
        quickSlotToChange = dataSlotUI.slotNum;

        int totalSlotsActive = 0;
        DataObject itemToDisplay = dataSlotUI.slotData;

        if (itemToDisplay != null)
        {
            gameMenuUI.interfaceGrid.SetActive(true);
            gameMenuUI.backButton.SetActive(true);
            gameMenuUI.equipmentUIGO.SetActive(false);

            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.weaponsInventory.Count)
                    {
                        totalSlotsActive += 1;
                        itemSlotUI.slotData = playerInventory.weaponsInventory[i];

                        myItemIcon.sprite = playerInventory.weaponsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if (dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellSlots[playerInventory.activeSpellSlotNumber];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.spellsInventory.Count)
                    {
                        totalSlotsActive += 1;
                        itemSlotUI.slotData = playerInventory.spellsInventory[i];

                        myItemIcon.sprite = playerInventory.spellsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if (dataSlotUI.slotData.GetType() == typeof(Gear))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.activeGear;

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.gearInventory.Count)
                    {
                        totalSlotsActive += 1;
                        itemSlotUI.slotData = playerInventory.gearInventory[i];

                        myItemIcon.sprite = playerInventory.gearInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if (dataSlotUI.slotData.GetType() == typeof(Consumable))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    bagUI.CountItemClasses(GetConsumables());

                    if (i < bagUI.typesOfItemsInInventory.Count)
                    {
                        for (int j = 0; j < GetConsumables().Count; j++)
                        {
                            if (bagUI.typesOfItemsInInventory[i] == GetConsumables()[j])
                            {
                                itemSlotUI.duplicates++;
                            }
                        }

                        itemSlotUI.duplicateCountText.gameObject.SetActive(true);
                        itemSlotUI.duplicateCountText.text = itemSlotUI.duplicates.ToString();

                        totalSlotsActive += 1;
                        itemSlotUI.slotData = bagUI.typesOfItemsInInventory[i];


                        myItemIcon.sprite = bagUI.typesOfItemsInInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if (dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.gearInventory.Count)
                    {
                        totalSlotsActive += 1;
                        itemSlotUI.slotData = playerInventory.offhandWeaponInventory[i];

                        myItemIcon.sprite = playerInventory.offhandWeaponInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }

            gameMenuUI.SwitchNextPageButton(totalSlotsActive);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.errorUIButtonSFX);
        }
    }

    private List<Item> GetConsumables()
    {
        List<Item> consumableInventory = playerInventory.itemInventory; //MAKING LIST OF ONLY CONSUMABLES
        for (int i = 0; i < consumableInventory.Count; i++)
        {
            if (consumableInventory[i].GetType() != typeof(Consumable))
            {
                consumableInventory.Remove(consumableInventory[i]);
            }
        }
        return consumableInventory;
    }
    #endregion

    #region Show Equipment Info
    public void SelectEquipmentToView(DataSlotUI dataSlotUI)
    {
        if(gameMenuUI.currentMenuIndex == 1 && gameMenuUI.currentSubmenuIndex == 0)
        {
            if (dataSlotUI.slotData != null)
            {
                int otherSlotNum;
                if (quickSlotToChange == 0)
                {
                    otherSlotNum = 1;
                }
                else
                {
                    otherSlotNum = 0;
                }

                if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
                {
                    if (dataSlotUI.slotData == playerInventory.weaponSlots[quickSlotToChange])
                    {
                        SwitchEquipButtonStatus(false, dataSlotUI);
                    }
                    else if (dataSlotUI.slotData != playerInventory.weaponSlots[otherSlotNum])
                    {
                        SwitchEquipButtonStatus(true, dataSlotUI);
                    }
                }
                else if (dataSlotUI.slotData.GetType() == typeof(Spell))
                {
                    if (dataSlotUI.slotData == playerInventory.spellSlots[quickSlotToChange])
                    {
                        SwitchEquipButtonStatus(false, dataSlotUI);
                    }
                    else if (dataSlotUI.slotData != playerInventory.spellSlots[otherSlotNum])
                    {
                        SwitchEquipButtonStatus(true, dataSlotUI);
                    }
                }
                else if (dataSlotUI.slotData.GetType() == typeof(Gear))
                {
                    if (dataSlotUI.slotData == playerInventory.activeGear)
                    {
                        SwitchEquipButtonStatus(false, dataSlotUI);
                    }
                    else
                    {
                        SwitchEquipButtonStatus(true, dataSlotUI);
                    }
                }
                else if (dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
                {
                    if (dataSlotUI.slotData == playerInventory.offhandSlots[quickSlotToChange])
                    {
                        SwitchEquipButtonStatus(false, dataSlotUI);
                    }
                    else if (dataSlotUI.slotData != playerInventory.offhandSlots[otherSlotNum])
                    {
                        SwitchEquipButtonStatus(true, dataSlotUI);
                    }
                }
                else if (dataSlotUI.slotData.GetType() == typeof(Consumable))
                {
                    if (dataSlotUI.slotData == playerInventory.consumableSlots[quickSlotToChange])
                    {
                        SwitchEquipButtonStatus(false, dataSlotUI);
                    }
                    else if (dataSlotUI.slotData != playerInventory.consumableSlots[otherSlotNum])
                    {
                        SwitchEquipButtonStatus(true, dataSlotUI);
                    }
                }
            }
        }
        
    }
    #endregion

    #region Equip/Unequip

    private void SwitchEquipButtonStatus(bool equipButtonIsOn, DataSlotUI dataSlotUI)
    {
        if(equipButtonIsOn) //Equip button turns on and uses the data
        {
            gameMenuUI.equipButton.SetActive(true);
            gameMenuUI.unequipButton.SetActive(false);
            gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = dataSlotUI.slotData;
        }
        else //Unequip button turns on and uses the data
        {
            gameMenuUI.equipButton.SetActive(false);
            gameMenuUI.unequipButton.SetActive(true);
            gameMenuUI.unequipButton.GetComponent<DataSlotUI>().slotData = dataSlotUI.slotData;
        }
    }

    public void Equip(DataSlotUI dataSlotUI)
    {
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.equipItemSFX);

        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            if (playerInventory.weaponSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if (playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] == playerInventory.weaponSlots[quickSlotToChange])
            {
                playerInventory.weaponSlots[playerInventory.activeWeaponSlotNumber] = (MeleeWeapon)dataSlotUI.slotData;
                playerMeleeHandler.SetMeleeParentOverride();
                playerMeleeHandler.LoadMeleeModel();
            }
            playerInventory.weaponSlots[quickSlotToChange] = (MeleeWeapon)dataSlotUI.slotData;

        }

        else if (dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            if (playerInventory.spellSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if (playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] == playerInventory.spellSlots[quickSlotToChange])
            {
                playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] = (Spell)dataSlotUI.slotData;
            }
            playerInventory.spellSlots[quickSlotToChange] = (Spell)dataSlotUI.slotData;
        }

        else if (dataSlotUI.slotData.GetType() == typeof(Gear))
        {
            if (playerInventory.activeGear == dataSlotUI.slotData)
            {
                return;
            }
            playerInventory.activeGear = (Gear)dataSlotUI.slotData;
            playerInventory.GetComponent<PlayerGearHandler>().InitializeGearEffect(playerInventory.activeGear.gearID);
        }

        else if (dataSlotUI.slotData.GetType() == typeof(Consumable))
        {
            if (playerInventory.consumableSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if (playerInventory.consumableSlots[playerInventory.activeConsumableSlotNumber] == playerInventory.consumableSlots[quickSlotToChange])
            {
                playerInventory.activeConsumableSlotNumber = quickSlotToChange;
            }
            playerInventory.consumableSlots[quickSlotToChange] = (Consumable)dataSlotUI.slotData;

        }

        else if (dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
        {
            if (playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] == dataSlotUI.slotData)
            {
                return;
            }
            if (playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] == playerInventory.offhandSlots[quickSlotToChange])
            {
                playerInventory.offhandSlots[playerInventory.activeOffhandWeaponSlotNumber] = (OffhandWeapon)dataSlotUI.slotData;
                playerOffhandHandler.LoadOffhandModel();
            }
            playerInventory.offhandSlots[quickSlotToChange] = (OffhandWeapon)dataSlotUI.slotData;

        }

        FindObjectOfType<QuickSlotUI>().UpdateQuickSlotIcons(playerInventory);
        gameMenuUI.unequipButton.GetComponent<DataSlotUI>().slotData = dataSlotUI.slotData;
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.unequipButton.SetActive(true);
    }

    public void Unequip(DataSlotUI dataSlotUI)
    {
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.unequipItemSFX);

        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            playerInventory.weaponSlots[quickSlotToChange] = null;
            playerMeleeHandler.DestroyMeleeModel();
        }
        else if(dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
        {
            playerInventory.offhandSlots[quickSlotToChange] = null;
            playerOffhandHandler.DestroyOffhandModel();
        }
        else if(dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            playerInventory.spellSlots[quickSlotToChange] = null;

        }
        else if(dataSlotUI.slotData.GetType() == typeof(Consumable))
        {
            playerInventory.consumableSlots[quickSlotToChange] = null;

        }
        else if(dataSlotUI.slotData.GetType() == typeof(Gear))
        {
            playerInventory.activeGear = null;

        }

        FindObjectOfType<QuickSlotUI>().UpdateQuickSlotIcons(playerInventory);
        gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = dataSlotUI.slotData;
        gameMenuUI.unequipButton.SetActive(false);
        gameMenuUI.equipButton.SetActive(true);
    }
    #endregion
}
