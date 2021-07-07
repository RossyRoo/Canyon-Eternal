using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;

    [Header("Interface Panel")]
    public Sprite emptyWindowSprite;
    public GameObject bagUIGO;
    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public int interfacePageIndex = 0;

    [Header("Info Panel")]
    public Item currentItem;
    public GameObject bagInfoPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public GameObject usableButtons;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Bag";
        bagUIGO.SetActive(true);

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
            OpenUsableInventory();
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

    #region Open Inventories
    public void OpenUsableInventory()
    {
        gameMenuUI.submenuNameText.text = "Usables";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.usableInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.usableInventory[i];
                myItemIcon.sprite = playerInventory.usableInventory[i].itemIcon;
                
                int thisItemSlotCount = 0;
                for (int j = 0; j < playerInventory.usableInventory.Count; j++)
                {
                    if(playerInventory.usableInventory[i].itemName == playerInventory.usableInventory[j].itemName)
                    {
                        thisItemSlotCount++;
                    }
                }

                itemSlotUI.itemCount.text = (thisItemSlotCount).ToString();

            }
            else
            {
                itemSlotUI.slotItem = null;
                myItemIcon.sprite = emptyWindowSprite;
            }
        }
    }

    public void OpenKeyInventory()
    {
        gameMenuUI.submenuNameText.text = "Keys";


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


    public void SelectDisplayItem(ItemSlotUI slotToSelect)
    {
        currentItem = slotToSelect.slotItem;

        if (currentItem != null)
        {
            bagInfoPanel.SetActive(true);
            itemIcon.sprite = currentItem.itemIcon;
            itemDescriptionText.text = currentItem.itemDescription;
            itemNameText.text = currentItem.itemName;

            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if(currentItem.GetType().Equals(typeof(Usable)))
            {
                usableButtons.SetActive(true);
            }
            else
            {
                usableButtons.SetActive(false);
            }
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.errorUIButtonSFX);
        }

    }

    /*
    public void SetQuickSlotUsable()
    {
        playerInventory.quickSlotUsable = (Usable)currentItem;

        Debug.Log("Setting current quick slot to "+ currentItem);
    }*/

    public void UseCurrentUsableItem()
    {
        FindObjectOfType<PlayerItemUser>().UseItemFromInventory((Usable)currentItem);
        OpenUsableInventory();
    }
}
