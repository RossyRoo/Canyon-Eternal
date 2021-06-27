using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapbookUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;


    public void OpenScrapbook(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Scrapbook";

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

    }

    public void OpenMap()
    {
        gameMenuUI.submenuNameText.text = "Map";
    }

    public void OpenJournal()
    {
        gameMenuUI.submenuNameText.text = "Journal";

    }

    public void OpenBestiary()
    {
        gameMenuUI.submenuNameText.text = "Bestiary";

    }
}
