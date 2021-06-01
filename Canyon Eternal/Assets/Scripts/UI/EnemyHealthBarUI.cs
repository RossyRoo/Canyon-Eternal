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
    public GameObject healthFill;
    Animator damageTextAnimator;

    public bool healthIsDecreasing = false;
    public bool criticalHitActive;

    public float timeUntilHealthBarDrains = 0.4f;
    public float healthDecreaseRate = 20;
    float timeUntilHealthBarIsHidden = 0f;
    float timeUntilDamageTextIsHidden = 0f;
    float timeUntilDestroyHealthBar = 0.5f;

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
                DestroyCanvasCoroutine();
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
        else
        {
            if (!damageText.gameObject.activeInHierarchy)
            {
                damageText.gameObject.SetActive(true);
                if (!criticalHitActive)
                {
                    damageTextAnimator.Play("DamageTextShake");
                }
                else
                {
                    damageTextAnimator.Play("CriticalTextShake");
                }
            }
        }
    }

    public IEnumerator SetHealthCoroutine(int health, bool isCriticalHit, int damage)
    {
        damageText.text = damage.ToString();
        timeUntilHealthBarIsHidden = 3f;
        timeUntilDamageTextIsHidden = 0.8f;

        damageSlider.value = currentHealth;
        currentHealth = health;
        healthSlider.value = currentHealth;

        criticalHitActive = isCriticalHit;

        yield return new WaitForSeconds(timeUntilHealthBarDrains);

        healthIsDecreasing = true;
    }

    private IEnumerator DestroyCanvasCoroutine()
    {
        Destroy(healthFill);

        yield return new WaitForSeconds(timeUntilDestroyHealthBar);

        Destroy(healthSlider.gameObject);
        Destroy(damageText.gameObject);
    }
}
