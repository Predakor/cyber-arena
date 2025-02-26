using UnityEngine;
using UnityEngine.UIElements;

public class VitalBars : MonoBehaviour {
    UIDocument _document;
    ProgressBar _healthBar;
    ProgressBar _shieldBar;

    [SerializeField] Health healthTarget;
    [SerializeField] string healthBarName;
    [SerializeField] string shieldBarName;

    int _currentHealth = 50;
    int _currentShield = 50;

    public void SetHealth(int health) {
        _currentHealth = Mathf.Clamp(0, health, (int)_healthBar.highValue);
        _healthBar.value = _currentHealth;
    }

    public void SetShield(int shield) {
        _currentShield = Mathf.Clamp(0, shield, (int)_shieldBar.highValue);
        _shieldBar.value = _currentShield;
    }


    void Awake() {
        if (_document == null) {
            _document = FindObjectOfType<UIDocument>();
        }
        if (healthTarget == null) {
            healthTarget = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Health>();
        }
    }


    void OnEnable() {
        var root = _document.rootVisualElement;
        _healthBar = root.Q<ProgressBar>(healthBarName);
        _shieldBar = root.Q<ProgressBar>(shieldBarName);

        if (healthTarget) {
            SetHealthTarget(healthTarget);
        }

    }

    void OnDisable() {
        healthTarget.OnHealthChange -= newHealth => SetHealth(newHealth);
        healthTarget.OnShieldChange -= newShield => SetShield(newShield);
    }


    public void ShowVitals(bool show = true) {
        int vissible = show ? 1 : 0;

        if (_healthBar != null) {
            _healthBar.style.opacity = vissible;
        }

        if (_shieldBar != null) {
            _shieldBar.style.opacity = vissible;
        }
    }

    public void SetHealthTarget(Health newHealthTarget) {
        if (healthTarget) {
            healthTarget.OnHealthChange -= newHealth => SetHealth(newHealth);
            healthTarget.OnShieldChange -= newShield => SetShield(newShield);
        }

        healthTarget = newHealthTarget;

        if (_healthBar != null) {
            _healthBar.value = healthTarget.CurrentHealth;
            _healthBar.highValue = healthTarget.MaxHealth;
            healthTarget.OnHealthChange += newHealth => SetHealth(newHealth);
        }

        if (_shieldBar != null) {
            _shieldBar.value = healthTarget.CurrentShield;
            _shieldBar.highValue = healthTarget.MaxShield;
            healthTarget.OnShieldChange += newShield => SetShield(newShield);
        }
    }

}
