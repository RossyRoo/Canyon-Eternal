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
    int currentMenuIndex = 0;
    int currentSubmenuIndex = 0;
    [HideInInspector]
    public bool gameMenuIsOpen;

    public GameObject gameMenusGO;
    public GameObject infoPanel;
    public GameObject[] buttons;
    public TextMeshProUGUI menuNameText;
    public TextMeshProUGUI submenuNameText;

    [Header("Interface Panels")]
    public GameObject interfaceBackground;
    public GameObject gearUIGO;
    public GameObject inventoryUIGO;
    public GameObject mapUIGO;
    public GameObject bestiaryUIGO;
    public GameObject contactsUIGO;
    public GameObject photosUIGO;
    public GameObject settingsUIGO;

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

        SwitchMenus();
    }

    public void ReverseGameMenuUI()
    {
        if(gameMenusGO.activeInHierarchy)
        {
            gameMenuIsOpen = false;
            playerManager.isInteractingWithUI = false;
            playerAnimatorHandler.animator.SetBool("isInteracting", false);
            gameMenusGO.SetActive(false);
            SFXPlayer.Instance.PlaySFXAudioClip(closeGameMenusSFX);
        }
        else
        {
            if (playerManager.isInteractingWithUI)
                return;

            gameMenuIsOpen = true;
            playerManager.isInteractingWithUI = true;
            playerAnimatorHandler.animator.SetBool("isInteracting", true);
            gameMenusGO.SetActive(true);
            SwitchMenus();
            SFXPlayer.Instance.PlaySFXAudioClip(openGameMenusSFX);
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

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        if (currentMenuIndex == 0)
        {
            cellphoneUI.CloseCellphone();
            bagUI.CloseBag();
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
                Debug.Log("this is a contact");
                cellphoneUI.activeContact = (Contact)itemSlotUI.slotData;
            }

            SFXPlayer.Instance.PlaySFXAudioClip(clickUIButtonSFX);
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(errorUIButtonSFX);
        }

    }
}

