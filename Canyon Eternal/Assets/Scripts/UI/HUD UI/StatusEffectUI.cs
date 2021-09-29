using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    public Image statusEffectIcon;
    public Sprite[] statusIconSprites;

    public void DisplayPlayerStatus(int damageType)
    {
        statusEffectIcon.sprite = statusIconSprites[damageType];
        statusEffectIcon.enabled = true;
    }

    public void HidePlayerStatus()
    {
        statusEffectIcon.sprite = null;
        statusEffectIcon.enabled = false;
    }
}
