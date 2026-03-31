using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Channels.Weapons;
using Systems.Guns;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

public sealed class WeaponPresenter : MonoBehaviour
{
    [SerializeField] private InputsChannel _inputChannel;
    [SerializeField] private WeaponChannel _weaponChannel;
    [SerializeField] private WeaponManager _weaponManager;

    private IAmmoEvents _currentAmmoEvents;

    private void OnEnable()
    {
        _inputChannel.Subscribe<InputEvents.Shoot>(OnShoot);
        _inputChannel.Subscribe<InputEvents.SelectWeapon>(OnSelectWeapon);
        RewireAmmoEvents(_weaponManager.CurrentWeapon);
    }

    private void OnDisable()
    {
        _inputChannel.Unsubscribe<InputEvents.Shoot>(OnShoot);
        _inputChannel.Unsubscribe<InputEvents.SelectWeapon>(OnSelectWeapon);
        UnsubscribeAmmoEvents(_currentAmmoEvents);
        _currentAmmoEvents = null;
    }

    private void OnShoot(InputEvents.Shoot e)
    {
        //_weaponManager.CurrentWeapon.Use(e.IsPressed);
    }

    private void OnSelectWeapon(InputEvents.SelectWeapon e)
    {
        _weaponManager.Equip(e.WeaponNumber);
        _weaponChannel.RaiseSelected(e.WeaponNumber);
        RewireAmmoEvents(_weaponManager.CurrentWeapon);
    }

    private void RewireAmmoEvents(IWeapon weapon)
    {
        UnsubscribeAmmoEvents(_currentAmmoEvents);
        _currentAmmoEvents = null;

        if (weapon is MonoBehaviour mb)
        {
            _currentAmmoEvents = mb.GetComponentInChildren<IAmmoEvents>();
            SubscribeAmmoEvents(_currentAmmoEvents);
        }
    }

    private void SubscribeAmmoEvents(IAmmoEvents ammoEvents)
    {
        if (ammoEvents == null)
        { return; }
        ammoEvents.OnAmmoChange += _weaponChannel.RaiseAmmoChanged;
        ammoEvents.OnReloadStart += _weaponChannel.RaiseReloadStarted;
        ammoEvents.OnReloadEnd += _weaponChannel.RaiseReloadFinished;
    }

    private void UnsubscribeAmmoEvents(IAmmoEvents ammoEvents)
    {
        if (ammoEvents == null)
        { return; }
        ammoEvents.OnAmmoChange -= _weaponChannel.RaiseAmmoChanged;
        ammoEvents.OnReloadStart -= _weaponChannel.RaiseReloadStarted;
        ammoEvents.OnReloadEnd -= _weaponChannel.RaiseReloadFinished;
    }

}

