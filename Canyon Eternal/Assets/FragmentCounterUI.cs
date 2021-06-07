using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FragmentCounterUI : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public TextMeshProUGUI totalText;
    public TextMeshProUGUI adjustmentText;

    float currentTotalDisplay;
    bool isCounting;

    private void Awake()
    {
        totalText.text = playerInventory.fragmentInventory.ToString();
    }

    public IEnumerator UpdateFragmentCountUI(int adjustment)
    {
        isCounting = true;

        adjustmentText.text = "+ " + adjustment.ToString();
        adjustmentText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        while (currentTotalDisplay < playerInventory.fragmentInventory)
        {
            currentTotalDisplay += Time.deltaTime * (adjustment + 10);
            currentTotalDisplay = Mathf.Clamp(currentTotalDisplay, 0f, playerInventory.fragmentInventory);
            totalText.text = Mathf.RoundToInt(currentTotalDisplay).ToString();
            yield return null;
        }

        isCounting = false;

        yield return new WaitForSeconds(1f);

        if(!isCounting)
        {
            adjustmentText.gameObject.SetActive(false);
        }
    }

}
