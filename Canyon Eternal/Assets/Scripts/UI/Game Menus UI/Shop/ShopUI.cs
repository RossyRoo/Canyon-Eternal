﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class ShopUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    BookUI bookUI;

    GameObject activeShopkeeperGO;
    List<Item> activeShopkeeperInventory;

    public bool shopIsOpen;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        bookUI = GetComponent<BookUI>();
    }

    public void OpenShop(string shopkeeperName, List<Item> thisShopkeeperInventory, GameObject thisShopkeeper)
    {
        //Find references
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        activeShopkeeperGO = thisShopkeeper;
        //Set UI elements
        gameMenuUI.menuNameText.text = shopkeeperName;
        gameMenuUI.submenuNameText.text = "Shop";
        gameMenuUI.mapUIGO.SetActive(false);
        gameMenuUI.interfacePanel.GetComponent<Image>().enabled = true;
        gameMenuUI.buyButton.SetActive(true);
        gameMenuUI.infoPanel.GetComponent<Image>().sprite = gameMenuUI.infoPanelSprites[1];
        gameMenuUI.interfacePanel.GetComponent<Image>().sprite = gameMenuUI.interfacePanelSprites[1];
        //Put player in UI mode
        playerManager.isInteractingWithUI = true;
        playerAnimatorHandler.animator.SetBool("isInteracting", true);
        gameMenuUI.gameMenusGO.SetActive(true);
        shopIsOpen = true;
        //Load Interface and Info panel
        gameMenuUI.RefreshGrid(true);
        activeShopkeeperInventory = thisShopkeeperInventory;
        RefreshShopInventory();

        //FX
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.openGameMenusSFX);
    }

    private void RefreshShopInventory()
    {
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


        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < activeShopkeeperInventory.Count)
            {
                itemSlotUI.slotData = activeShopkeeperInventory[i];

                myItemIcon.sprite = activeShopkeeperInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
        }
    }
    
    public void CloseShop()
    {
        if(shopIsOpen)
        {
            shopIsOpen = false;

            gameMenuUI.buyButton.SetActive(false);

            activeShopkeeperGO.GetComponent<Usable>().enabled = true;
        }

    }
    

    public void BuyItem(DataSlotUI dataSlotUI)
    {
        Item selectedItem = (Item)dataSlotUI.slotData;


        if (playerInventory.fragmentInventory >= selectedItem.itemValue)
        {
            if (!selectedItem.isRare ||
                selectedItem.isRare && !playerInventory.itemInventory.Contains(selectedItem))
            {
                playerInventory.itemInventory.Add(selectedItem);
                playerInventory.AdjustFragmentInventory(-selectedItem.itemValue);
                if(selectedItem.isRare)
                {
                    gameMenuUI.RefreshGrid(true);
                    RefreshShopInventory();
                    gameMenuUI.infoPanel.SetActive(false);
                }
            }
            else
            {
                Debug.Log("You already bought this");
            }
        }
        else
        {
            Debug.Log("You cant afford this");

        }

    }
}
