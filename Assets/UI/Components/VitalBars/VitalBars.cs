using UI.Core;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class VitalBars : UIComponentBehaviour
{
    [SerializeField] private MonoBehaviour _healthTargetSource;//for inspector reference,
    [SerializeField] private bool _visibleOnStart = true;

    [UIElement("health-bar")] private readonly ProgressBar _healthBar;
    [UIElement("shield-bar")] private readonly ProgressBar _shieldBar;

    private IHealthMonitor _target;

    protected override void OnUIEnabled()
    {
        ShowVitals(_visibleOnStart);
    }

    protected override void OnUIDisabled()
    {
        CleanupCurrentTarget();
    }


    public void ShowVitals(bool show = true)
    {
        int vissible = show ? 1 : 0;

        if (_healthBar != null)
        {
            _healthBar.style.opacity = vissible;
        }

        if (_shieldBar != null)
        {
            _shieldBar.style.opacity = vissible;
        }
    }

    public void SetHealthTarget(IHealthMonitor newTarget)
    {
        if (_target != null)
        {
            CleanupCurrentTarget(); //clear old events
        }

        _target = newTarget;
        _healthTargetSource = (MonoBehaviour)newTarget;

        if (_healthBar is not null)
        {
            _healthBar.value = _target.CurrentHealth;
            _healthBar.highValue = _target.MaxHealth;
            _target.OnHealthChange += SetHealth;
            _target.OnMaxHealthChange += SetMaxHealth;
        }

        if (_shieldBar is not null)
        {
            _shieldBar.value = _target.CurrentShield;
            _shieldBar.highValue = _target.MaxShield;
            _target.OnShieldChange += SetShield;
            _target.OnMaxShieldChange += SetMaxShield;
        }
    }

    private void CleanupCurrentTarget()
    {
        if (_target == null)
        {
            return;
        }

        _target.OnHealthChange -= SetHealth;
        _target.OnMaxHealthChange -= SetMaxHealth;
        _target.OnShieldChange -= SetShield;
        _target.OnMaxShieldChange -= SetMaxShield;

        _target = null;
    }

    private void SetShield(int value) => _shieldBar.value = value;
    private void SetHealth(int value) => _healthBar.value = value;
    private void SetMaxHealth(int value) => _healthBar.highValue = value;
    private void SetMaxShield(int value) => _shieldBar.highValue = value;

}
