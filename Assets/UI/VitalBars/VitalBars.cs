using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class VitalBars : MonoBehaviour
{
    [SerializeField] private string healthBarName;
    [SerializeField] private string shieldBarName;

    [SerializeField] private UIDocument _document;
    [SerializeField] private MonoBehaviour healthTargetSource;

    private ProgressBar _healthBar;
    private ProgressBar _shieldBar;

    private IHealthMonitor _target;


    private void Awake()
    {
        gameObject.EnsureComponent(out _document);
    }


    private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _healthBar ??= root.Q<ProgressBar>(healthBarName);
        _shieldBar ??= root.Q<ProgressBar>(shieldBarName);
    }

    private void OnDisable()
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
        healthTargetSource = (MonoBehaviour)newTarget;

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
