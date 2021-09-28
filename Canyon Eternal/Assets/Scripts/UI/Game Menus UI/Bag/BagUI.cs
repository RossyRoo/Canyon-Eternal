using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;
    ConsumableHandler consumableHandler;
    EquipmentUI equipmentUI;


    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        equipmentUI = GetComponent<EquipmentUI>();
        consumableHandler = FindObjectOfType<ConsumableHandler>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

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
            equipmentUI.OpenEquipment();
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
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.unequipButton.SetActive(false);
        gameMenuUI.backButton.SetActive(false);
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.equipmentUIGO.SetActive(false);
    }

    #region Inventory

    public void OpenItemInventory()
    {
        gameMenuUI.submenuNameText.text = "Inventory";
        gameMenuUI.equipmentUIGO.SetActive(false);
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.backButton.SetActive(false);

        gameMenuUI.RefreshGrid(true);

        DisplayItemInventoryGrid();

    }

    private void DisplayItemInventoryGrid()
    {
        int totalSlotsActive = 0;


        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < gameMenuUI.CountItemTypesInList(playerInventory.itemInventory).Count)
            {
                totalSlotsActive += 1;
                itemSlotUI.slotData = gameMenuUI.CountItemTypesInList(playerInventory.itemInventory)[i];
                myItemIcon.sprite = gameMenuUI.CountItemTypesInList(playerInventory.itemInventory)[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);

                
                itemSlotUI.duplicates = 0; 

                if(!gameMenuUI.CountItemTypesInList(playerInventory.itemInventory)[i].isRare) //Calculate duplicates
                {
                    gameMenuUI.CountItemDuplicatesInList(itemSlotUI.slotData, itemSlotUI, playerInventory.itemInventory);

                }

            }

        }
        gameMenuUI.SwitchNextPageButton(totalSlotsActive);
    }

    public void UseConsumable(DataSlotUI dataSlotUI)
    {
        PlayerStats playerStats = playerInventory.GetComponent<PlayerStats>();
        int numOfConsumedItem = 0;

        consumableHandler.HandlePlayerConsumable((Consumable)dataSlotUI.slotData, playerStats);

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
        gameMenuUI.backButton.SetActive(false);


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
    
}