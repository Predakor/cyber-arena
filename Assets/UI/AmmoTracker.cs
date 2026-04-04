using Systems.Channels.Weapons;
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

        if (_ammoLabel is null)
        {
            Debug.LogError("MIssing ammoLabel in UI Document");
        }

        _weaponChannel.Subscribe<WeaponEvents.AmmoChanged>(HandleAmmoChanged);
        _weaponChannel.Subscribe<WeaponEvents.ReloadStarted>(HandleReloadStarted);
        _weaponChannel.Subscribe<WeaponEvents.ReloadFinished>(HandleReloadFinished);
    }

    private void OnDisable()
    {
        _weaponChannel.Unsubscribe<WeaponEvents.AmmoChanged>(HandleAmmoChanged);
        _weaponChannel.Unsubscribe<WeaponEvents.ReloadStarted>(HandleReloadStarted);
        _weaponChannel.Unsubscribe<WeaponEvents.ReloadFinished>(HandleReloadFinished);
    }

    private void HandleAmmoChanged(WeaponEvents.AmmoChanged e)
    {
        if (_ammoLabel == null)
        {
            return;
        }
        _ammoLabel.text = e.Reserve.HasValue ? $"{e.Current} / {e.Reserve}" : $"{e.Current}";
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
