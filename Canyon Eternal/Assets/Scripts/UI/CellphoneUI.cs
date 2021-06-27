using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;

    public void OpenCellphone(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Cell";

        if (currentSubmenuIndex == 0)
        {
            OpenMessages();
        }
        else if (currentSubmenuIndex == 1)
        {
            OpenPhotos();
        }
        else
        {
            OpenSettings();
        }
    }

    public void CloseCellphone()
    {

    }

    public void OpenMessages()
    {
        gameMenuUI.submenuNameText.text = "Messages";

    }

    public void OpenPhotos()
    {
        gameMenuUI.submenuNameText.text = "Photos";

    }

    public void OpenSettings()
    {
        gameMenuUI.submenuNameText.text = "Settings";

    }
}
