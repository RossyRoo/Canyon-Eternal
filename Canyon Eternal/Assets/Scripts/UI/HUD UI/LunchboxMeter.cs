using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LunchboxMeter : MonoBehaviour
{
    //public Image nextHealItemSlot;

    //public Sprite[] fullHealItems;

    public Sprite lunchSprite;

    public TextMeshProUGUI currentLunchboxCapacityText;


    public void SetMaxLunchbox(int maxLunchboxCapacity)
    {
        currentLunchboxCapacityText.gameObject.SetActive(true);
        currentLunchboxCapacityText.text = maxLunchboxCapacity.ToString();

        //nextHealItemSlot.GetComponentInParent<RawImage>().texture = fullWindow.texture;

        //nextHealItemSlot.enabled = true;
        //nextHealItemSlot.sprite = fullHealItems[maxLunchboxCapacity - 1];
    }

    public void SetCurrentLunchBox(int currentLunchboxCapacity)
    {
        currentLunchboxCapacityText.text = currentLunchboxCapacity.ToString();

        if(currentLunchboxCapacity == 0)
        {
            //currentLunchboxCapacityText.gameObject.SetActive(false);
            //nextHealItemSlot.enabled = false;
            //nextHealItemSlot.GetComponentInParent<RawImage>().texture = emptyWindow.texture;
        }
        else
        {
            currentLunchboxCapacityText.gameObject.SetActive(true);
            //nextHealItemSlot.enabled = true;
            //nextHealItemSlot.sprite = fullHealItems[currentLunchboxCapacity - 1];
        }
    }
}
