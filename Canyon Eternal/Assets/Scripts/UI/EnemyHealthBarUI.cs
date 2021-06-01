using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider damageSlider;
    public TextMeshProUGUI damageText;
    Animator damageTextAnimator;

    bool healthIsDecreasing = false;

    float timeUntilHealthBarDrains = 0.4f;
    float healthDecreaseRate = 20;
    float timeUntilHealthBarIsHidden = 0f;
    float timeUntilDamageTextIsHidden = 0f;

    public int currentHealth;

    private void Awake()
    {
        damageTextAnimator = damageText.GetComponent<Animator>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        damageSlider.maxValue = maxHealth;
        damageSlider.value = maxHealth;
        currentHealth = maxHealth;
    }

    private void DecreaseHealthOverTime()
    {
        if(healthIsDecreasing)
        {
            damageSlider.value -= Time.deltaTime * healthDecreaseRate;

            if (damageSlider.value <= healthSlider.value)
            {
                healthIsDecreasing = false;
                damageSlider.value = healthSlider.value;
            }
        }
        else
        {
            return;
        }
    }


    private void Update()
    {
        if(healthSlider != null)
        {
            timeUntilHealthBarIsHidden = timeUntilHealthBarIsHidden - Time.deltaTime;
            timeUntilDamageTextIsHidden = timeUntilDamageTextIsHidden - Time.deltaTime;

            DisplayHealthBar();
            DecreaseHealthOverTime();

            DisplayDamageText();

            if (damageSlider.value <= 0)
            {
                Destroy(healthSlider.gameObject);
                Destroy(damageText.gameObject);
            }
        }
    }

    private void DisplayHealthBar()
    {
        if (timeUntilHealthBarIsHidden <= 0)
        {
            timeUntilHealthBarIsHidden = 0;
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            if (!healthSlider.gameObject.activeInHierarchy)
            {
                healthSlider.gameObject.SetActive(true);
            }
        }
    }

    private void DisplayDamageText()
    {
        if (timeUntilDamageTextIsHidden <= 0)
        {
            timeUntilDamageTextIsHidden = 0;
            damageText.gameObject.SetActive(false);
        }
    }

    public IEnumerator SetHealthCoroutine(int health, bool isCriticalHit, int damage)
    {
        damageText.text = damage.ToString();
        timeUntilHealthBarIsHidden = 3f;
        timeUntilDamageTextIsHidden = 0.8f;

        damageText.gameObject.SetActive(true);
        if (!isCriticalHit)
        {
            damageTextAnimator.Play("DamageTextShake");
        }
        else
        {
            damageTextAnimator.Play("CriticalTextShake");
        }

        damageSlider.value = currentHealth;
        currentHealth = health;
        healthSlider.value = currentHealth;

        yield return new WaitForSeconds(timeUntilHealthBarDrains);

        healthIsDecreasing = true;
    }
}
