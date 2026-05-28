using Systems.Guns;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class AmmoTracker : MonoBehaviour
{
    [SerializeField] private WeaponChannel _weaponChannel;
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Label _ammoLabel;
    [SerializeField] private VisualElement _reloadIndicator;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        _ammoLabel = root.Q<Label>("ammo-field");
        _reloadIndicator = root.Q<VisualElement>("reload-indicator");

        Debug.Assert(_reloadIndicator is null, "Reload indicator not found in UI Document", this);

        _weaponChannel.Subscribe<WeaponEvents.AmmoChanged>(HandleAmmoChanged, destroyCancellationToken);
        _weaponChannel.Subscribe<WeaponEvents.ReloadStarted>(HandleReloadStarted, destroyCancellationToken);
        _weaponChannel.Subscribe<WeaponEvents.ReloadFinished>(HandleReloadFinished, destroyCancellationToken);
    }

    private void HandleAmmoChanged(WeaponEvents.AmmoChanged e)
    {
        if (_ammoLabel == null)
        {
            return;
        }

        _ammoLabel.text = e.Reserve.HasValue
            ? $"{e.Current} / {e.Reserve}"
            : $"{e.Current}";
    }

    private void HandleReloadStarted(WeaponEvents.ReloadStarted e)
    {
        if (_reloadIndicator == null)
        {
            return;
        }
        _reloadIndicator.style.display = DisplayStyle.Flex;
    }

    private void HandleReloadFinished(WeaponEvents.ReloadFinished e)
    {
        if (_reloadIndicator == null)
        {
            return;
        }
        _reloadIndicator.style.display = DisplayStyle.None;
    }
}
