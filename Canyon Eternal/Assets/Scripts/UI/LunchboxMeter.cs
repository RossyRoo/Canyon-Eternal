using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunchboxMeter : MonoBehaviour
{
    public Image[] healItems;

    public Sprite[] fullHealItems;
    public Sprite[] emptyHealItems;

    public void SetMaxLunchbox(int maxLunchboxCapacity)
    {
        for (int i = 0; i < healItems.Length; i++)
        {
            healItems[i].sprite = fullHealItems[i];
            if (i < maxLunchboxCapacity)
            {
                healItems[i].enabled = true;
            }
            else
            {
                healItems[i].enabled = false;
            }
        }
    }

    public void SetCurrentLunchBox(int currentLunchboxCapacity)
    {
        for (int i = 0; i < healItems.Length; i++)
        {
            if (i < currentLunchboxCapacity)
            {
                healItems[i].sprite = fullHealItems[i];
            }
            else
            {
                healItems[i].sprite = emptyHealItems[i];
            }
        }
    }
}
