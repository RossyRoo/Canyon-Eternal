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
    public EnemyManager enemyManager;

    bool healthIsDecreasing = false;

    float timeUntilHealthBarDrains = 0.4f;
    float healthDecreaseRate = 100;
    float timeUntilHealthBarIsHidden = 0f;
    float timeUntilDamageTextIsHidden = 0f;

    [HideInInspector]public float currentHealth;

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
            FollowEnemy();

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

    public void DisplayHealthBar()
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

    public IEnumerator SetHealthCoroutine(bool isLosingHealth, float health, bool isCriticalHit, float damage)
    {
        timeUntilHealthBarIsHidden = 3f;
        timeUntilDamageTextIsHidden = 0.8f;

        //DAMAGE TEXT
        damageText.text = damage.ToString();
        damageText.gameObject.SetActive(true);

        if (isLosingHealth)
        {
            if (!isCriticalHit)
            {
                damageTextAnimator.Play("DamageTextShake");
            }
            else
            {
                damageTextAnimator.Play("CriticalTextShake");
            }
        }
        else
        {
            damageTextAnimator.Play("HealTextShake");
        }


        damageSlider.value = currentHealth;
        currentHealth = health;
        healthSlider.value = currentHealth;

        yield return new WaitForSeconds(timeUntilHealthBarDrains);

        healthIsDecreasing = true;
    }

    private void FollowEnemy()
    {
        transform.position = enemyManager.transform.position;
    }
}
