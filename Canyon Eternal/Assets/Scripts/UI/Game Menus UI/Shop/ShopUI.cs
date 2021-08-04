
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

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        bookUI = GetComponent<BookUI>();
    }

    public void OpenShop(string shopkeeperName, List<Item> shopInventory, GameObject thisShopkeeper)
    {
        //Find references
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        activeShopkeeperGO = thisShopkeeper;
        //Set UI elements
        gameMenuUI.menuNameText.text = shopkeeperName;
        gameMenuUI.submenuNameText.text = "Shop";
        bookUI.worldMapUIGO.SetActive(false);
        gameMenuUI.interfaceBackground.GetComponent<Image>().enabled = true;
        gameMenuUI.buyButton.SetActive(true);
        //Put player in UI mode
        playerManager.isInteractingWithUI = true;
        playerAnimatorHandler.animator.SetBool("isInteracting", true);
        gameMenuUI.gameMenusGO.SetActive(true);
        //Load Interface and Info panel
        gameMenuUI.RefreshGrid(true);

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

            if (i < shopInventory.Count)
            {
                itemSlotUI.slotData = shopInventory[i];

                myItemIcon.sprite = shopInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
        }
        //FX
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.openGameMenusSFX);
    }

    public void CloseShop()
    {
        gameMenuUI.buyButton.SetActive(false);

        //ObjectPool objectPool = FindObjectOfType<ObjectPool>();
        //Instantiate(activeShopkeeperGO.GetComponent<ShopTrigger>().shopkeeperGoodbye, objectPool.transform.position, Quaternion.identity);

        activeShopkeeperGO.GetComponent<Usable>().enabled = true;
    }

    public void BuyItem(DataSlotUI dataSlotUI)
    {
        Item selectedItem = (Item)dataSlotUI.slotData;

        if(playerInventory.fragmentInventory >= selectedItem.itemValue)
        {
            playerInventory.AdjustFragmentInventory(-selectedItem.itemValue);
        }
    }
}
