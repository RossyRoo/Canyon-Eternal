using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartMeter : MonoBehaviour
{
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void SetMaxHearts(int maxHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = fullHeart;
            if (i < maxHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void SetCurrentHealth(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
