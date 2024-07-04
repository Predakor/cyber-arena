using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Health healthManager;
    public Slider healthSlider;
    public Slider shieldSlider;

    void Start() {
        if (healthManager == null) { return; }

        if (healthSlider) {
            healthSlider.value = healthManager.CurrentHealth;
            healthSlider.minValue = 0;
            healthSlider.maxValue = healthManager.MaxHealth;
        }

        if (shieldSlider) {
            shieldSlider.value = healthManager.Shield;
            shieldSlider.minValue = 0;
            shieldSlider.maxValue = healthManager.MaxShield;
        }

    }

    void Update() {
        if (healthSlider.value != healthManager.CurrentHealth) {
            healthSlider.value = healthManager.CurrentHealth;
        }
        if (shieldSlider.value != healthManager.Shield) {
            shieldSlider.value = healthManager.Shield;
        }
    }
    public void UpdateHealthSlider(int newValue) {
        if (!healthSlider) { return; }

        if (newValue > healthManager.MaxHealth) {
            newValue = healthManager.MaxHealth;
        }
        healthSlider.value = newValue;
    }

    public void UpdateShieldSlider(int newValue) {
        if (!shieldSlider) { return; }

        if (newValue > healthManager.MaxShield) {
            newValue = healthManager.MaxShield;
        }
        shieldSlider.value = newValue;
    }
}
