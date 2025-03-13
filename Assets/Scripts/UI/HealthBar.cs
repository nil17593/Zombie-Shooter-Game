using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Color lowHealthColor;
    [SerializeField] private Color highHealthColor;
    [SerializeField] private Image fillrectImage;
    public void SetHealth(float currentHealth, float maxHealth)
    {
        //slider.gameObject.SetActive(currentHealth < maxHealth);
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        fillrectImage.color = Color.Lerp(lowHealthColor, highHealthColor, slider.normalizedValue);
    }
}
