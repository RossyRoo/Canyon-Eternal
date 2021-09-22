using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerOffhandHandler playerOffhandHandler;
    ConsumableHandler playerEffectsHandler;

    public GameObject quickWeaponSlot1Button;
    public GameObject quickWeaponSlot2Button;
    public GameObject quickOffhandSlot1Button;
    public GameObject quickOffhandSlot2Button;
    public GameObject quickSpellSlot1Button;
    public GameObject quickSpellSlot2Button;
    public GameObject quickConsumableSlot1Button;
    public GameObject quickConsumableSlot2Button;
    public GameObject activeGearButton;

    public List<Item> typesOfItemsInInventory;

    public int quickSlotToChange;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        playerEffectsHandler = FindObjectOfType<ConsumableHandler>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMeleeHandler = FindObjectOfType<PlayerMeleeHandler>();
        playerOffhandHandler = FindObjectOfType<PlayerOffhandHandler>();

        gameMenuUI.menuNameText.text = "Bag";

        for (int i = 0; i < gameMenuUI.interfacePages.Length; i++)
        {
            if (i == 0)
            {
                gameMenuUI.interfacePages[i].SetActive(true);
            }
            else
            {
                gameMenuUI.interfacePages[i].SetActive(false);
            }
        }

        if (currentSubmenuIndex == 0)
        {
            OpenEquipment();
        }
        else if (currentSubmenuIndex == 1)
        {
            OpenItemInventory();
        }
        else
        {
            OpenArtifacts();
        }
    }

    public void CloseBag()
    {
        //OpenEquipment();
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.equipmentOverviewButton.SetActive(false);
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.equipmentUIGO.SetActive(false);
    }

    #region Inventory

    public void CountItemClasses()
    {
        typesOfItemsInInventory.Clear();

        for (int i = 0; i < playerInventory.itemInventory.Count; i++)
        {
            if(!typesOfItemsInInventory.Contains(playerInventory.itemInventory[i]))
            {
                typesOfItemsInInventory.Add(playerInventory.itemInventory[i]);
            }
        }
    }

    public void OpenItemInventory()
    {
        gameMenuUI.submenuNameText.text = "Inventory";
        gameMenuUI.equipmentUIGO.SetActive(false);
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.equipmentOverviewButton.SetActive(false);

        gameMenuUI.RefreshGrid(true);

        CountItemClasses();
        DisplayItemInventoryGrid();

    }

    private void DisplayItemInventoryGrid()
    {
        int totalSlotsActive = 0;

        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < typesOfItemsInInventory.Count)
            {
                totalSlotsActive += 1;
                itemSlotUI.slotData = typesOfItemsInInventory[i];
                myItemIcon.sprite = typesOfItemsInInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);

                //Calculate duplicates
                itemSlotUI.duplicates = 0;
                for (int j = 0; j < playerInventory.itemInventory.Count; j++)
                {
                    if (itemSlotUI.slotData == playerInventory.itemInventory[j])
                    {
                        itemSlotUI.duplicates++;
                    }
                }

                if (itemSlotUI.duplicates != 0)
                {
                    itemSlotUI.duplicateCountText.gameObject.SetActive(true);
                    itemSlotUI.duplicateCountText.text = itemSlotUI.duplicates.ToString();
                }

            }

        }
        gameMenuUI.SwitchNextPageButton(totalSlotsActive);
    }

    public void UseConsumable(DataSlotUI dataSlotUI)
    {
        PlayerStats playerStats = playerInventory.GetComponent<PlayerStats>();
        int numOfConsumedItem = 0;

        playerEffectsHandler.HandlePlayerConsumable((Consumable)dataSlotUI.slotData, playerStats);

        for (int i = 0; i < playerInventory.itemInventory.Count; i++)
        {
            if (playerInventory.itemInventory[i].dataName == dataSlotUI.slotData.dataName)
            {
                numOfConsumedItem++;
            }
        }

        if (numOfConsumedItem <= 0)
        {
            gameMenuUI.infoPanel.SetActive(false);
        }
        OpenItemInventory();


    }

    #endregion

    #region Artifacts

    public void OpenArtifacts()
    {
        int totalSlotsActive = 0;

        gameMenuUI.submenuNameText.text = "Artifacts";
        gameMenuUI.equipmentUIGO.SetActive(false);
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.equipmentOverviewButton.SetActive(false);


        gameMenuUI.RefreshGrid(true);

        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.artifactInventory.Count)
            {
                totalSlotsActive += 1;
                itemSlotUI.slotData = playerInventory.artifactInventory[i];

                myItemIcon.sprite = playerInventory.artifactInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotData = null;
                myItemIcon.sprite = gameMenuUI.emptyWindowSprite;
            }
        }
        gameMenuUI.SwitchNextPageButton(totalSlotsActive);
    }

    #endregion

    #region Equipment
    public void OpenEquipment()
    {
        gameMenuUI.submenuNameText.text = "Equipment";
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.infoPanel.SetActive(false);
        gameMenuUI.equipmentUIGO.SetActive(true);

        quickWeaponSlot1Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickWeaponSlots[0];
        quickWeaponSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickWeaponSlots[0].dataIcon;
        quickWeaponSlot2Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickWeaponSlots[1];
        quickWeaponSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickWeaponSlots[1].dataIcon;


        quickOffhandSlot1Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickOffhandSlots[0];
        quickOffhandSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickOffhandSlots[0].dataIcon;
        quickOffhandSlot2Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickOffhandSlots[1];
        quickOffhandSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickOffhandSlots[1].dataIcon;

        quickSpellSlot1Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickSpellSlots[0];
        quickSpellSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickSpellSlots[0].dataIcon;
        quickSpellSlot2Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickSpellSlots[1];
        quickSpellSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickSpellSlots[1].dataIcon;

        quickConsumableSlot1Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickItemSlots[0];
        quickConsumableSlot1Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickItemSlots[0].dataIcon;
        quickConsumableSlot2Button.GetComponent<DataSlotUI>().slotData =
            playerInventory.quickItemSlots[1];
        quickConsumableSlot2Button.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.quickItemSlots[1].dataIcon;

        activeGearButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.activeGear;
        activeGearButton.GetComponent<DataSlotUI>().icon.sprite =
            playerInventory.activeGear.dataIcon;

        gameMenuUI.equipButton.SetActive(true);
    }

    public void SelectEquipmentToChange(DataSlotUI dataSlotUI)
    {
        //FIND THE QUICK SLOT NUM TO CHANGE HERE
        int totalSlotsActive = 0;
        DataObject itemToDisplay = dataSlotUI.slotData;

        if (itemToDisplay != null)
        {
            gameMenuUI.interfaceGrid.SetActive(true);
            gameMenuUI.equipmentOverviewButton.SetActive(true);
            gameMenuUI.equipmentUIGO.SetActive(false);

            gameMenuUI.infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
            gameMenuUI.infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
            gameMenuUI.infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;
            gameMenuUI.infoPanel.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if(dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.activeWeapon;

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
            else if(dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.activeSpell;

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
            else if(dataSlotUI.slotData.GetType() == typeof(Gear))
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
            else if(dataSlotUI.slotData.GetType() == typeof(Consumable))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.activeConsumable;

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.itemInventory.Count)
                    {
                        if(playerInventory.itemInventory[i].GetType() == typeof(Consumable))
                        {
                            totalSlotsActive += 1;
                            itemSlotUI.slotData = playerInventory.itemInventory[i];

                            myItemIcon.sprite = playerInventory.itemInventory[i].dataIcon;
                            myItemIcon.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if(dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.activeOffhandWeapon;

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

    public void SelectEquipmentToView(DataSlotUI dataSlotUI)
    {

        if(dataSlotUI.slotData != null)
        {
            if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (MeleeWeapon)dataSlotUI.slotData;
            }
            else if (dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (Spell)dataSlotUI.slotData;
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Gear))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (Gear)dataSlotUI.slotData;
            }
            else if(dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (OffhandWeapon)dataSlotUI.slotData;
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Consumable))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (Consumable)dataSlotUI.slotData;
            }
        }
    }

    public void ChangeEquipment(DataSlotUI dataSlotUI)
    {
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.equipItemSFX);

        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            if(playerInventory.quickWeaponSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if(playerInventory.activeWeapon == playerInventory.quickWeaponSlots[quickSlotToChange])
            {
                playerInventory.activeWeapon = (MeleeWeapon)dataSlotUI.slotData;
                playerMeleeHandler.DestroyMeleeModel();
                playerMeleeHandler.SetMeleeParentOverride();
                playerMeleeHandler.LoadMeleeModel();
            }
            playerInventory.quickWeaponSlots[quickSlotToChange] = (MeleeWeapon)dataSlotUI.slotData;

        }

        else if (dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            if (playerInventory.quickSpellSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if(playerInventory.activeSpell == playerInventory.quickSpellSlots[quickSlotToChange])
            {
                playerInventory.activeSpell = (Spell)dataSlotUI.slotData;
            }
            playerInventory.quickSpellSlots[quickSlotToChange] = (Spell)dataSlotUI.slotData;
        }

        else if(dataSlotUI.slotData.GetType() == typeof(Gear))
        {
            if (playerInventory.activeGear == dataSlotUI.slotData)
            {
                return;
            }
            playerInventory.activeGear = (Gear)dataSlotUI.slotData;
            playerInventory.GetComponent<PlayerGearHandler>().InitializeGearEffect(playerInventory.activeGear.gearID);
        }

        else if(dataSlotUI.slotData.GetType() == typeof(Consumable))
        {
            if (playerInventory.quickItemSlots[quickSlotToChange] == dataSlotUI.slotData)
            {
                return;
            }
            if(playerInventory.activeConsumable == playerInventory.quickItemSlots[quickSlotToChange])
            {
                playerInventory.activeConsumable = (Consumable)dataSlotUI.slotData;
            }
            playerInventory.quickItemSlots[quickSlotToChange] = (Consumable)dataSlotUI.slotData;

        }

        else if(dataSlotUI.slotData.GetType() == typeof(OffhandWeapon))
        {
            if (playerInventory.activeOffhandWeapon == dataSlotUI.slotData)
            {
                return;
            }
            if(playerInventory.activeOffhandWeapon == playerInventory.quickItemSlots[quickSlotToChange])
            {
                playerInventory.activeOffhandWeapon = (OffhandWeapon)dataSlotUI.slotData;
                playerOffhandHandler.DestroyOffhandModel();
                playerOffhandHandler.LoadOffhandModel();
            }
            playerInventory.quickOffhandSlots[quickSlotToChange] = (OffhandWeapon)dataSlotUI.slotData;

        }

        FindObjectOfType<QuickSlotUI>().UpdateQuickSlotIcons(playerInventory);
    }
    #endregion
}