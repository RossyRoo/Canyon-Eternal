using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;

    public BagUI bagUI;
    public CellphoneUI cellphoneUI;
    public ScrapbookUI scrapbookUI;

    public GameObject gameMenusGO;
    public TextMeshProUGUI menuNameText;

    int currentMenuIndex = 0;
    int currentSubmenuIndex = 0;
    [HideInInspector]
    public bool gameMenuIsOpen;


    private void Awake()
    {
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
        }
    }


    #region Cycle Menus

    public void CycleMenuRight()
    {
        if(currentMenuIndex == 0)
        {
            currentMenuIndex++;
        }
        else if(currentMenuIndex == 1)
        {
            currentMenuIndex++;
        }
        else
        {
            currentMenuIndex = 0;
        }

        SwitchMenus();
    }

    public void CycleMenuLeft()
    {
        if (currentMenuIndex == 0)
        {
            currentMenuIndex = 2;
        }
        else if (currentMenuIndex == 1)
        {
            currentMenuIndex--;
        }
        else
        {
            currentMenuIndex--;
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

        SwitchMenus();
    }



    #endregion

    private void SwitchMenus()
    {
        if (currentMenuIndex == 0)
        {
            cellphoneUI.CloseCellphone();
            bagUI.CloseBag();
            scrapbookUI.OpenScrapbook(currentSubmenuIndex);
        }
        else if (currentMenuIndex == 1)
        {
            scrapbookUI.CloseScrapbook();
            cellphoneUI.CloseCellphone();
            bagUI.OpenBag(currentSubmenuIndex);
        }
        else
        {
            bagUI.CloseBag();
            scrapbookUI.CloseScrapbook();
            cellphoneUI.OpenCellphone(currentSubmenuIndex);
        }
    }
}
