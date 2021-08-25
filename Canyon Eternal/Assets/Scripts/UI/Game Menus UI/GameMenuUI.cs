using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    BagUI bagUI;
    CellphoneUI cellphoneUI;
    BookUI bookUI;
    ShopUI shopUI;
    FastTravelUI fastTravelUI;
    int currentMenuIndex = 0;
    int currentSubmenuIndex = 0;
    //[HideInInspector]
    public bool gameMenuIsOpen;

    [Header("GENERAL")]
    public GameObject gameMenusGO;
    public TextMeshProUGUI menuNameText;
    public TextMeshProUGUI submenuNameText;

    [Header("INTERFACE")]
    public GameObject interfaceBackground;
    public GameObject interfaceGrid;
    public GameObject[] interfacePages;
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
    public GameObject buyButton;
    public GameObject equipmentOverviewButton;
    public GameObject fastTravelButton;
    public GameObject useButton;

    [Header("SFX")]
    public AudioClip closeGameMenusSFX;
    public AudioClip openGameMenusSFX;
    public AudioClip openScrapbookSFX;
    public AudioClip openCellPhoneSFX;
    public AudioClip openBagSFX;
    public AudioClip switchScrapbookSubMenuSFX;
    public AudioClip switchCellphoneSubMenuSFX;
    public AudioClip switchBagSubMenuSFX;
    public AudioClip clickUIButtonSFX;
    public AudioClip errorUIButtonSFX;
    public AudioClip phoneRingSFX;


    private void Awake()
    {
        bagUI = GetComponent<BagUI>();
        bookUI = GetComponent<BookUI>();
        cellphoneUI = GetComponent<CellphoneUI>();
        gameMenusGO.SetActive(false);
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        shopUI = GetComponent<ShopUI>();
        fastTravelUI = GetComponent<FastTravelUI>();

        SwitchMenus();
    }

    public void ReverseGameMenuUI(bool isShopping)
    {
        if(gameMenusGO.activeInHierarchy)
        {
            gameMenuIsOpen = false;
            playerManager.isInteractingWithUI = false;
            playerAnimatorHandler.animator.SetBool("isInteracting", false);

            bagUI.CloseBag();
            cellphoneUI.CloseCellphone();
            bookUI.CloseBook();
            shopUI.CloseShop();
            fastTravelUI.CloseFastTravel();

            gameMenusGO.SetActive(false);
            SFXPlayer.Instance.PlaySFXAudioClip(closeGameMenusSFX);
        }
        else
        {
            if (playerManager.isInteractingWithUI)
                return;

            playerManager.isInteractingWithUI = true;
            playerAnimatorHandler.animator.SetBool("isInteracting", true);
            gameMenusGO.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(openGameMenusSFX);

            if (!isShopping)
            {
                gameMenuIsOpen = true;
                SwitchMenus();
            }
        }
    }


    #region Cycle Menus

    public void CycleMenuRight()
    {
        if(currentMenuIndex == 0)
        {
            currentMenuIndex++;
            SFXPlayer.Instance.PlaySFXAudioClip(openBagSFX);
        }
        else if(currentMenuIndex == 1)
        {
            currentMenuIndex++;
            SFXPlayer.Instance.PlaySFXAudioClip(openCellPhoneSFX);
        }
        else
        {
            currentMenuIndex = 0;
            SFXPlayer.Instance.PlaySFXAudioClip(openScrapbookSFX);
        }

        SwitchMenus();
    }

    public void CycleMenuLeft()
    {
        if (currentMenuIndex == 0)
        {
            currentMenuIndex = 2;
            SFXPlayer.Instance.PlaySFXAudioClip(openCellPhoneSFX);
        }
        else if (currentMenuIndex == 1)
        {
            currentMenuIndex--;
            SFXPlayer.Instance.PlaySFXAudioClip(openScrapbookSFX);
        }
        else
        {
            currentMenuIndex--;
            SFXPlayer.Instance.PlaySFXAudioClip(openBagSFX);
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
            SFXPlayer.Instance.PlaySFXAudioClip(switchScrapbookSubMenuSFX);
        }
        else if (currentMenuIndex == 1)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(switchBagSubMenuSFX);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(switchCellphoneSubMenuSFX);
        }
    }

    #endregion

    private void SwitchMenus()
    {
        interfaceBackground.GetComponent<Image>().enabled = true;

        if (currentMenuIndex == 0)
        {
            cellphoneUI.CloseCellphone();
            bagUI.CloseBag();

            if(currentSubmenuIndex == 2) //DOING THIS BECAUSE WE ARENT USING THE JOURNAL
            {
                currentSubmenuIndex = 0;
            }

            bookUI.OpenBook(currentSubmenuIndex);
        }
        else if (currentMenuIndex == 1)
        {
            cellphoneUI.CloseCellphone();
            bookUI.CloseBook();
            bagUI.OpenBag(currentSubmenuIndex);
        }
        else
        {
            bookUI.CloseBook();
            bagUI.CloseBag();
            cellphoneUI.OpenCellphone(currentSubmenuIndex);
        }

        infoPanel.SetActive(false);
    }

    public void SelectItem(DataSlotUI itemSlotUI)
    {
        DataObject itemToDisplay = itemSlotUI.slotData;

        if(itemToDisplay != null)
        {
            infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
            infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
            infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;

            infoPanel.SetActive(true);

            if (itemSlotUI.slotData.GetType() == typeof(Contact))
            {
                cellphoneUI.activeContact = (Contact)itemSlotUI.slotData;
            }
            else if (itemSlotUI.slotData.GetType() == typeof(Item))
            {
                buyButton.GetComponent<DataSlotUI>().slotData = itemSlotUI.slotData;
            }
            else if(itemSlotUI.slotData.GetType() == typeof(Room))
            {
                fastTravelButton.GetComponent<DataSlotUI>().slotData = itemSlotUI.slotData;
            }

            //Check if Item is Usable
            if(itemSlotUI.slotData.GetType() == typeof(Consumable))
            {
                useButton.SetActive(true);
                useButton.GetComponent<DataSlotUI>().slotData = itemSlotUI.slotData;
            }
            else
            {
                useButton.SetActive(false);
            }

            SFXPlayer.Instance.PlaySFXAudioClip(clickUIButtonSFX);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(errorUIButtonSFX);
        }
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

        SFXPlayer.Instance.PlaySFXAudioClip(clickUIButtonSFX);
    }

    public void RefreshGrid(bool turnBackOn)
    {
        for (int i = 0; i < interfaceGridSlots.Length; i++)
        {
            interfaceGridSlots[i].GetComponent<DataSlotUI>().slotData = null;
            interfaceGridSlots[i].GetComponent<DataSlotUI>().duplicateCountText.gameObject.SetActive(false);
            interfaceGridSlots[i].GetComponent<Image>().sprite = emptyWindowSprite;
        }
        interfaceGrid.SetActive(false);

        if(turnBackOn == true)
        {
            interfaceGrid.SetActive(true);
        }
    }
}

