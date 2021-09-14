using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaNameText : MonoBehaviour
{
    public TextMeshProUGUI areaNameText;
    public Animator areaNameTextAnimator;


    public IEnumerator ShowAreaName(string areaName)
    {
        areaNameText.text = areaName;
        areaNameTextAnimator.Play("Fade In");

        yield return new WaitForSeconds(2.25f);

        areaNameTextAnimator.Play("Fade Out");
    }
}
