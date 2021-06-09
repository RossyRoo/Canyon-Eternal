﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public Image[] cards;

    public void SetMaxStamina(float maxImagination)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Animator>().Play("StaminaIdle");

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
                if(cards[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StaminaIdle"))
                {
                    Debug.Log("This heart is idle");
                }
                else
                {
                    cards[i].GetComponent<Animator>().Play("StaminaHeal");
                }
            }
            else
            {
                cards[i].GetComponent<Animator>().Play("StaminaBreak");
            }
        }
    }

}
