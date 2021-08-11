using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerManager playerManager;
    BookUI bookUI;
    PlayerAnimatorHandler playerAnimatorHandler;


    public bool fastTravelIsOpen;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        bookUI = GetComponent<BookUI>();

    }

    public void OpenFastTravel()
    {
        //Find references
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();

        //Set UI elements
        gameMenuUI.menuNameText.text = "Travel";
        gameMenuUI.submenuNameText.text = "Travel";
        bookUI.worldMapUIGO.SetActive(false);
        gameMenuUI.interfaceBackground.GetComponent<Image>().enabled = true;
        gameMenuUI.fastTravelButton.SetActive(true);

        //Put player in UI mode
        playerManager.isInteractingWithUI = true;
        playerAnimatorHandler.animator.SetBool("isInteracting", true);
        gameMenuUI.gameMenusGO.SetActive(true);
        fastTravelIsOpen = true;

        //Load Interface and Info panel
        gameMenuUI.RefreshGrid(true);

    }

    public void CloseFastTravel()
    {
        if (fastTravelIsOpen)
        {
            fastTravelIsOpen = false;

            gameMenuUI.fastTravelButton.SetActive(false);
        }
    }
}
