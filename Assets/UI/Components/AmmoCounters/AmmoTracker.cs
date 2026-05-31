using Systems.Guns;
using UI.Core;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public sealed class AmmoTracker : UIComponentBehaviour
{
    [UIElement("ammo-field")]
    [SerializeField] private Label _ammoLabel;
    [UIElement("reload-indicator")]
    [SerializeField] private VisualElement _reloadIndicator;

    [SerializeField] private WeaponChannel _weaponChannel;

    protected override void OnUIEnabled()
    {
        _weaponChannel.Subscribe<WeaponEvents.AmmoChanged>(HandleAmmoChanged, UiCancellationToken);
        _weaponChannel.Subscribe<WeaponEvents.ReloadStarted>(HandleReloadStarted, UiCancellationToken);
        _weaponChannel.Subscribe<WeaponEvents.ReloadFinished>(HandleReloadFinished, UiCancellationToken);
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
