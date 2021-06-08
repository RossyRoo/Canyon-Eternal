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

    public void SetCurrentHealth(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].GetComponent<Animator>().Play("HealthIdle");
            }
            else
            {
                hearts[i].GetComponent<Animator>().Play("HealthBreak");
            }
        }
    }
}
