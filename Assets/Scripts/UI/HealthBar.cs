using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperGaming.ZombieShooter.UI
{
    /// <summary>
    /// this is helthbar common for all type of objects
    /// we just have to give the current health and max health
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("Health Bar settings")]
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
}