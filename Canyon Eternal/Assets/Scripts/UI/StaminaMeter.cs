using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public Image[] cards;

    public Sprite fullCard;
    public Sprite emptyCard;

    public void SetMaxStamina(float maxImagination)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].sprite = fullCard;
            if (i < maxImagination)
            {
                cards[i].enabled = true;
            }
            else
            {
                cards[i].enabled = false;
            }
        }
    }

    public void SetCurrentStamina(float currentImagination)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < Mathf.Floor(currentImagination))
            {
                cards[i].sprite = fullCard;
            }
            else
            {
                cards[i].sprite = emptyCard;
            }
        }
    }

}
