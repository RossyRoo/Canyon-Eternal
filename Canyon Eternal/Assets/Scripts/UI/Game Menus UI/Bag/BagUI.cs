using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;

    [Header("Interface Panel")]
    public Sprite emptyWindowSprite;
    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public int interfacePageIndex = 0;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        gameMenuUI = GetComponent<GameMenuUI>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Bag";

        for (int i = 0; i < interfacePages.Length; i++)
        {
            if (i == 0)
            {
                interfacePages[i].SetActive(true);
            }
            else
            {
                interfacePages[i].SetActive(false);
            }
        }

        if (currentSubmenuIndex == 0)
        {
            OpenEquipment();
        }
        else if (currentSubmenuIndex == 1)
        {
            OpenKeyInventory();
        }
        else
        {
            OpenTreasureInventory();
        }
    }

    public void CloseBag()
    {
        gameMenuUI.inventoryUIGO.SetActive(false);
    }

    #region Open Inventories
    public void OpenEquipment()
    {
        gameMenuUI.submenuNameText.text = "Gear";

        gameMenuUI.inventoryUIGO.SetActive(false);
    }

    public void OpenKeyInventory()
    {
        gameMenuUI.submenuNameText.text = "Keys";
        gameMenuUI.inventoryUIGO.SetActive(true);


        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.keyInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.keyInventory[i];

                myItemIcon.sprite = playerInventory.keyInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotItem = null;
                myItemIcon.sprite = emptyWindowSprite;
            }
        }
    }

    public void OpenTreasureInventory()
    {
        gameMenuUI.submenuNameText.text = "Treasures";
        gameMenuUI.inventoryUIGO.SetActive(true);

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.treasureInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.treasureInventory[i];

                myItemIcon.sprite = playerInventory.treasureInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotItem = null;
                myItemIcon.sprite = emptyWindowSprite;
            }
        }
    }
    #endregion

    public void CycleInterfacePages()
    {
        if (interfacePageIndex < interfacePages.Length - 1)
        {
            interfacePageIndex++;
        }
        else
        {
            interfacePageIndex = 0;
        }

        for (int i = 0; i < interfacePages.Length; i++)
        {
            if (i == interfacePageIndex)
            {
                interfacePages[i].SetActive(true);
            }
            else
            {
                interfacePages[i].SetActive(false);
            }
        }

        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);
    }

    /*
    public void UseCurrentUsableItem()
    {
        FindObjectOfType<PlayerItemUser>().UseItemFromInventory((Usable)currentItem);
        OpenUsableInventory();
        //If there is no more of this item, you need to close the info panel
    }

    
    public void SetQuickSlotUsable()
    {
        playerInventory.quickSlotUsable = (Usable)currentItem;

        Debug.Log("Setting current quick slot to " + currentItem);
    }*/
}
