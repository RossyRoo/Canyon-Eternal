using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public Image[] cards;

    public void SetMaxStamina(float maxStamina)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Animator>().Play("StaminaIdle");

            if (i < maxStamina)
            {
                cards[i].enabled = true;
            }
            else
            {
                cards[i].enabled = false;
            }
        }
    }

    public void SetCurrentStamina(float currentStamina, float startingMaxStamina)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if(i < startingMaxStamina)
            {
                if (i < Mathf.Floor(currentStamina))
                {
                    if (!cards[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StaminaIdle"))
                    {
                        cards[i].GetComponent<Animator>().Play("StaminaHeal");
                    }

                }
                else
                {
                    cards[i].GetComponent<Animator>().Play("StaminaBreak");
                }
            }
            else
            {
                if (i < Mathf.Floor(currentStamina))
                {
                    if (!cards[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BuffIdle"))
                    {
                        cards[i].GetComponent<Animator>().Play("BuffHeal");
                    }

                }
                else
                {
                    cards[i].GetComponent<Animator>().Play("BuffBreak");
                }
            }

        }
    }

}
