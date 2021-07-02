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

    [Header("Map")]
    public GameObject worldMapUIGO;
    public GameObject areaMapUIGO;
    public GameObject playerIcon;
    public GameObject[] areaMaps;


    public void OpenScrapbook(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = null;
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

        PrepareWorldMap();
    }


    public void PrepareWorldMap()
    {
        areaMapUIGO.SetActive(false);
        worldMapUIGO.SetActive(true);

        Debug.Log("Clearing cover froma areas discovered");

        //Place player icon at current area
        //Remove cover from areas you have discovered
        //If it's the first time the map has been seen since area was discovered, get rid of that cover and play animation of clouds dispersing
        //animate and activate button to check out that area
    }

    public void PrepareAreaMap()
    {
        worldMapUIGO.SetActive(false);
        areaMapUIGO.SetActive(true);

        Debug.Log("Clearing cover froma rooms discovered");

        //Place player icon at current room
        //Remove cover from rooms you have discovered
    }

    public void SwitchBetweenMaps()
    {
        if(worldMapUIGO.activeInHierarchy)
        {
            PrepareAreaMap();
        }
        else
        {
            PrepareWorldMap();
        }
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
