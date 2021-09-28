using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    EquipmentUI equipmentUI;
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    BagUI bagUI;
    CellphoneUI cellphoneUI;
    BookUI bookUI;
    ShopUI shopUI;
    FastTravelUI fastTravelUI;
    public int currentMenuIndex = 0;
    public int currentSubmenuIndex = 0;
    //[HideInInspector]
    public bool gameMenuIsOpen;

    [Header("GENERAL")]
    public GameObject gameMenusGO;
    public TextMeshProUGUI menuNameText;
    public TextMeshProUGUI submenuNameText;
    public Image menuTypeIcon;

    [Header("INTERFACE")]
    public GameObject interfacePanel;
    public GameObject interfaceGrid;
    public GameObject[] interfacePages;
    public GameObject nextPageButton;
    [HideInInspector]public int interfacePageIndex = 0;
    public Sprite emptyWindowSprite;
    public GameObject [] interfaceGridSlots;
    public GameObject equipmentUIGO;
    public GameObject mapUIGO;
    public GameObject settingsUIGO;

    [Header("INFO")]
    public GameObject infoPanel;
    public GameObject callButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject buyButton;
    public GameObject backButton;
    public GameObject fastTravelButton;
    public GameObject useButton;
    public GameObject purchasedBanner;

    [Header("SPRITES")]
    public Sprite[] menuTypeSprite;


    [Header("SFX")]
    public AudioClip closeGameMenusSFX;
    public AudioClip openGameMenusSFX;
    public AudioClip openScrapbookSFX;
    public AudioClip openCellPhoneSFX;
    public AudioClip openBagSFX;
    public AudioClip switchScrapbookSubMenuSFX;
    public AudioClip switchCellphoneSubMenuSFX;
    public AudioClip switchBagSubMenuSFX;
    public AudioClip backButtonSFX;
    public AudioClip clickUIButtonSFX;
    public AudioClip nextPageUIButtonSFX;
    public AudioClip errorUIButtonSFX;
    public AudioClip equipItemSFX;
    public AudioClip unequipItemSFX;
    public AudioClip purchaseItemSFX;
    public AudioClip phoneRingSFX;


    private void Awake()
    {
        bagUI = GetComponent<BagUI>();
        bookUI = GetComponent<BookUI>();
        equipmentUI = GetComponent<EquipmentUI>();
        cellphoneUI = GetComponent<CellphoneUI>();
        gameMenusGO.SetActive(false);
        playerManager = FindObjectOfType<PlayerManager>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        shopUI = GetComponent<ShopUI>();
        fastTravelUI = GetComponent<FastTravelUI>();

    }

    private void Start()
    {
        SwitchMenus();
    }

    public void ReverseGameMenuUI(bool isShopping)
    {
        if(gameMenusGO.activeInHierarchy)
        {
            gameMenuIsOpen = false;
            playerManager.isInteractingWithUI = false;

            bagUI.CloseBag();
            cellphoneUI.CloseCellphone();
            bookUI.CloseBook();
            shopUI.CloseShop();
            fastTravelUI.CloseFastTravel();

            gameMenusGO.SetActive(false);
            SFXPlayer.Instance.PlaySFXAudioClip(closeGameMenusSFX, 0.15f);
        }
        else
        {
            if (playerManager.isInteractingWithUI)
                return;

            playerManager.isInteractingWithUI = true;
            gameMenusGO.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(openGameMenusSFX, 0.15f);

            if (!isShopping)
            {
                gameMenuIsOpen = true;
                SwitchMenus();
            }
        }
    }

    private void SwitchMenus()
    {
        interfacePanel.GetComponent<Image>().enabled = true;

        if (currentMenuIndex == 0)
        {
            cellphoneUI.CloseCellphone();
            bagUI.CloseBag();

            if (currentSubmenuIndex == 2) //DOING THIS BECAUSE WE ARENT USING THE JOURNAL
            {
                currentSubmenuIndex = 0;
            }

            bookUI.OpenBook(currentSubmenuIndex);

            menuTypeIcon.sprite = menuTypeSprite[0];
        }
        else if (currentMenuIndex == 1)
        {
            cellphoneUI.CloseCellphone();
            bookUI.CloseBook();
            bagUI.OpenBag(currentSubmenuIndex);

            menuTypeIcon.sprite = menuTypeSprite[1];
        }
        else
        {
            bookUI.CloseBook();
            bagUI.CloseBag();
            cellphoneUI.OpenCellphone(currentSubmenuIndex);

            menuTypeIcon.sprite = menuTypeSprite[2];
        }

        infoPanel.SetActive(false);
    }

    #region Cycle Menus

    public void CycleMenuRight()
    {
        if(currentMenuIndex == 0)
        {
            currentMenuIndex++;
            SFXPlayer.Instance.PlaySFXAudioClip(openBagSFX, 0.15f);
        }
        else if(currentMenuIndex == 1)
        {
            currentMenuIndex++;
            SFXPlayer.Instance.PlaySFXAudioClip(openCellPhoneSFX, 0.15f);
        }
        else
        {
            currentMenuIndex = 0;
            SFXPlayer.Instance.PlaySFXAudioClip(openScrapbookSFX, 0.15f);
        }

        SwitchMenus();
    }

    public void CycleMenuLeft()
    {
        if (currentMenuIndex == 0)
        {
            currentMenuIndex = 2;
            SFXPlayer.Instance.PlaySFXAudioClip(openCellPhoneSFX, 0.15f);
        }
        else if (currentMenuIndex == 1)
        {
            currentMenuIndex--;
            SFXPlayer.Instance.PlaySFXAudioClip(openScrapbookSFX, 0.15f);
        }
        else
        {
            currentMenuIndex--;
            SFXPlayer.Instance.PlaySFXAudioClip(openBagSFX, 0.15f);
        }

        SwitchMenus();
    }

    #endregion

    #region Cycle Submenus

    public void CycleSubmenuRight()
    {
        if (currentSubmenuIndex == 0)
        {
            currentSubmenuIndex++;
        }
        else if (currentSubmenuIndex == 1)
        {
            currentSubmenuIndex++;
        }
        else
        {
            currentSubmenuIndex = 0;
        }

        PlaySubMenuSwitchingSFX();
        SwitchMenus();
    }

    public void CycleSubmenuLeft()
    {
        if (currentSubmenuIndex == 0)
        {
            currentSubmenuIndex = 2;
        }
        else if (currentMenuIndex == 1)
        {
            currentSubmenuIndex--;
        }
        else
        {
            currentSubmenuIndex--;
        }

        PlaySubMenuSwitchingSFX();
        SwitchMenus();
    }

    private void PlaySubMenuSwitchingSFX()
    {
        if (currentMenuIndex == 0)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(switchScrapbookSubMenuSFX, 0.15f);
        }
        else if (currentMenuIndex == 1)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(switchBagSubMenuSFX, 0.15f);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(switchCellphoneSubMenuSFX, 0.15f);
        }
    }

    #endregion

    #region Buttons

    public void SelectDataSlot(DataSlotUI itemSlotUI)
    {

        if(itemSlotUI.slotData != null)
        {
            //INFO PANEL
            infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemSlotUI.slotData.dataName;
            infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemSlotUI.slotData.dataDescription;
            infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemSlotUI.slotData.dataIcon;
            infoPanel.SetActive(true);

            //TURN OFF BUTTONS
            callButton.SetActive(false);
            buyButton.SetActive(false);
            fastTravelButton.SetActive(false);
            useButton.SetActive(false);

            if (itemSlotUI.slotData.GetType() == typeof(Contact))
            {
                SelectContact((Contact)itemSlotUI.slotData);
            }
            else if (shopUI.shopIsOpen)
            {
                SelectShopItem((Item)itemSlotUI.slotData);
            }
            else if(itemSlotUI.slotData.GetType() == typeof(Room))
            {
                SelectFastTravelRoom((Room)itemSlotUI.slotData);
            }
            else if(itemSlotUI.slotData.GetType() == typeof(Consumable) && !shopUI.shopIsOpen)
            {
                SelectConsumableItem((Consumable)itemSlotUI.slotData);
            }

            SFXPlayer.Instance.PlaySFXAudioClip(clickUIButtonSFX);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(errorUIButtonSFX);
        }
    }

    private void SelectContact(Contact contactToCall)
    {
        callButton.SetActive(true);
        cellphoneUI.activeContact = contactToCall;
    }

    private void SelectShopItem(Item itemToBuy)
    {
        if (playerInventory.itemInventory.Contains(itemToBuy) && itemToBuy.isRare
            || itemToBuy.GetType() == typeof(Spell) && playerInventory.spellsInventory.Contains((Spell)itemToBuy)
            || itemToBuy.GetType() == typeof(MeleeWeapon) && playerInventory.weaponsInventory.Contains((MeleeWeapon)itemToBuy))
        {
            buyButton.SetActive(false);
            purchasedBanner.SetActive(true);
        }
        else
        {
            buyButton.SetActive(true);
            purchasedBanner.SetActive(false);
            buyButton.GetComponent<DataSlotUI>().slotData = itemToBuy;
        }
    }

    private void SelectConsumableItem(Consumable consumableToUse)
    {
        if(currentSubmenuIndex == 1)
        {
            useButton.SetActive(true);
            useButton.GetComponent<DataSlotUI>().slotData = consumableToUse;
        }
    }

    private void SelectFastTravelRoom(Room fastTravelRoomDestination)
    {
        fastTravelButton.SetActive(true);
        fastTravelButton.GetComponent<DataSlotUI>().slotData = fastTravelRoomDestination;
    }


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

        SFXPlayer.Instance.PlaySFXAudioClip(nextPageUIButtonSFX);
    }

    public void SwitchNextPageButton(int numberOfSlotsActive)
    {
        if(numberOfSlotsActive <= 12)
        {
            nextPageButton.SetActive(false);
            if(interfacePageIndex > 0)
            {
                CycleInterfacePages();
            }
        }
        else
        {
            nextPageButton.SetActive(true);
        }
    }

    public void RefreshGrid(bool turnBackOn)
    {
        for (int i = 0; i < interfaceGridSlots.Length; i++)
        {
            interfaceGridSlots[i].GetComponent<DataSlotUI>().slotData = null;
            interfaceGridSlots[i].GetComponent<DataSlotUI>().duplicates = 0;
            interfaceGridSlots[i].GetComponent<DataSlotUI>().duplicateCountText.gameObject.SetActive(false);
            interfaceGridSlots[i].GetComponent<DataSlotUI>().icon.gameObject.SetActive(false);
        }
        interfaceGrid.SetActive(false);

        if(turnBackOn == true)
        {
            interfaceGrid.SetActive(true);
        }
    }

    public void BackButton()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(backButtonSFX);

        if (currentMenuIndex == 0)
        {
            bookUI.SwitchToWorldMap();
        }
        else if (currentMenuIndex == 1)
        {
            equipmentUI.OpenEquipment();
        }
    }

    #endregion

    #region Duplicates

    public List<Item> CountItemTypesInList(List<Item> itemList)
    {
        itemList = itemList.OrderBy(x => x.dataName).ToList();

        List<Item> itemTypes = new List<Item>();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (!itemTypes.Contains(itemList[i]))
            {
                itemTypes.Add(itemList[i]);
            }
        }
        return itemTypes;
    }

    public void CountItemDuplicatesInList(DataObject itemToCount, DataSlotUI dataSlotToChange, List<Item> itemList)
    {
        for (int j = 0; j < itemList.Count; j++)
        {
            if (itemToCount == itemList[j])
            {
                dataSlotToChange.duplicates++;
            }
        }

        dataSlotToChange.duplicateCountText.gameObject.SetActive(true);
        dataSlotToChange.duplicateCountText.text = dataSlotToChange.duplicates.ToString();
    }

    #endregion
}

