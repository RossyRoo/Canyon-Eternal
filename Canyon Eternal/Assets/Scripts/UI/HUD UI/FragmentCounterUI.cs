using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FragmentCounterUI : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public TextMeshProUGUI totalText;
    public TextMeshProUGUI adjustmentText;

    public AudioClip counterGoingUpSFX;

    float currentTotalDisplay;
    float currentAdjustmentDisplay;
    bool isCounting;

    private void Awake()
    {
        totalText.text = playerInventory.fragmentInventory.ToString();
    }

    public IEnumerator UpdateFragmentCountUI(int adjustment)
    {
        isCounting = true;
        currentAdjustmentDisplay += adjustment;

        adjustmentText.text = "+ " + currentAdjustmentDisplay.ToString();
        adjustmentText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        InvokeRepeating("CounterSFX", 0f, 0.05f);

        while (currentTotalDisplay < playerInventory.fragmentInventory)
        {
            currentTotalDisplay += Time.deltaTime * (adjustment + 10);
            currentTotalDisplay = Mathf.Clamp(currentTotalDisplay, 0f, playerInventory.fragmentInventory);
            totalText.text = Mathf.RoundToInt(currentTotalDisplay).ToString();


            currentAdjustmentDisplay -= Time.deltaTime * (adjustment + 10);
            currentAdjustmentDisplay = Mathf.Clamp(currentAdjustmentDisplay, 0f, playerInventory.fragmentInventory);
            adjustmentText.text = Mathf.RoundToInt(currentAdjustmentDisplay).ToString();

            yield return null;
        }

        CancelInvoke();

        isCounting = false;

        yield return new WaitForSeconds(1f);

        if(!isCounting)
        {
            adjustmentText.gameObject.SetActive(false);
        }
    }

    private void CounterSFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(counterGoingUpSFX, 0.02f);
    }
}
