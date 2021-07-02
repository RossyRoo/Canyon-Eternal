using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrapbookUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;
    public TextMeshProUGUI submenuNameText;
    public GameObject scrapbookUIGO;
    public GameObject mapUIGO;
    public GameObject journalUIGO;
    public GameObject bestiaryUIGO;


    public void OpenScrapbook(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Scrapbook";
        scrapbookUIGO.SetActive(true);

        if(currentSubmenuIndex == 0)
        {
            OpenMap();
        }
        else if(currentSubmenuIndex == 1)
        {
            OpenJournal();
        }
        else
        {
            OpenBestiary();
        }
    }

    public void CloseScrapbook()
    {
        scrapbookUIGO.SetActive(false);
    }

    #region Map
    public void OpenMap()
    {
        submenuNameText.text = "Map";
        bestiaryUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        mapUIGO.SetActive(true);

    }

    #endregion

    public void OpenJournal()
    {
        submenuNameText.text = "Journal";
        bestiaryUIGO.SetActive(false);
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(true);

    }

    public void OpenBestiary()
    {
        submenuNameText.text = "Bestiary";
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        bestiaryUIGO.SetActive(true);

    }
}
