using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaNameText : MonoBehaviour
{
    public TextMeshProUGUI areaNameText;
    Animator areaNameTextAnimator;

    private void Awake()
    {
        areaNameTextAnimator = GetComponent<Animator>();
    }

    public IEnumerator ShowAreaName(string areaName)
    {
        areaNameText.text = areaName;
        areaNameTextAnimator.Play("Fade In");

        yield return new WaitForSeconds(2.25f);

        areaNameTextAnimator.Play("Fade Out");
    }
}
