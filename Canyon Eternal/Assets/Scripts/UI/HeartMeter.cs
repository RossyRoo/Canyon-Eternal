using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartMeter : MonoBehaviour
{
    public Image[] hearts;

    public void SetMaxHearts(int maxHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Animator>().Play("HealthIdle");

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

    public void SetCurrentHealth(int currentHearts, int startingMaxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < startingMaxHealth)
            {
                if (i < currentHearts)
                {
                    if (hearts[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HealthIdle"))
                    {
                        hearts[i].GetComponent<Animator>().Play("HealthIdle");
                    }
                    else
                    {
                        hearts[i].GetComponent<Animator>().Play("HealthHeal");
                    }
                }
                else
                {
                    hearts[i].GetComponent<Animator>().Play("HealthBreak");
                }
            }
            else
            {
                if(i < currentHearts)
                {
                    if (hearts[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BuffIdle"))
                    {
                        hearts[i].GetComponent<Animator>().Play("BuffIdle");
                    }
                    else
                    {
                        hearts[i].GetComponent<Animator>().Play("BuffHeal");
                    }
                }
                else
                {
                    hearts[i].GetComponent<Animator>().Play("BuffBreak");
                }

            }

        }
    }
}
