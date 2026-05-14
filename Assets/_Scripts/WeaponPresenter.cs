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
    private IGun _currentGun;

    private void OnEnable()
    {
        _inputChannel.Subscribe<InputEvents.Shoot>(OnShoot);
        _inputChannel.Subscribe<InputEvents.SelectWeapon>(OnSelectWeapon);
        _weaponChannel.Subscribe<WeaponEvents.Reconfigured>(ReconfigureWeaponHandler);
        RewireGun(_weaponManager.CurrentWeapon);
    }

    private void OnDisable()
    {
        _inputChannel.Unsubscribe<InputEvents.Shoot>(OnShoot);
        _inputChannel.Unsubscribe<InputEvents.SelectWeapon>(OnSelectWeapon);
        _weaponChannel.Unsubscribe<WeaponEvents.Reconfigured>(ReconfigureWeaponHandler);
        UnsubscribeAmmoEvents(_currentAmmoEvents);
        UnsubscribeStatsEvents(_currentGun);
        _currentAmmoEvents = null;
        _currentGun = null;
    }

    private void OnShoot(InputEvents.Shoot e)
    {
        _weaponManager.CurrentWeapon.Use(e.IsPressed);
    }

    private void OnSelectWeapon(InputEvents.SelectWeapon e)
    {
        _weaponManager.Equip(e.WeaponNumber);
        _weaponChannel.RaiseSelected(e.WeaponNumber);
        RewireGun(_weaponManager.CurrentWeapon);
    }

    private void RewireGun(IWeapon weapon)
    {
        UnsubscribeAmmoEvents(_currentAmmoEvents);
        UnsubscribeStatsEvents(_currentGun);
        _currentAmmoEvents = null;
        _currentGun = null;

        if (weapon is null)
        {
            Debug.LogError("expected weapon type but got null", this);
            return;
        }

        if (weapon is not IGun gun)
        {
            Debug.LogError("expected weapon type but got " + weapon.GetType(), this);
            return;
        }

        _currentGun = gun;
        _currentAmmoEvents = gun.AmmoEvents;

        SubscribeAmmoEvents(_currentAmmoEvents);
        SubscribeStatsEvents(_currentGun);

        // Push current state immediately on connect
        _weaponChannel.RaiseStatsChanged(gun.Stats);
        _weaponChannel.RaiseModulesChanged(gun.Modules);
        _weaponChannel.RaiseAmmoChanged(gun.CurrentState.CurrentAmmo);
    }

    private void ReconfigureWeaponHandler(WeaponEvents.Reconfigured x)
    {
        _weaponManager.CurrentWeapon.Configure(x.Config);

        if (_weaponManager.CurrentWeapon is IGun gun)
        {
            SwapAmmoEvents(gun);
        }
    }

    private void SubscribeStatsEvents(IGun gun)
    {
        if (gun == null)
        {
            return;
        }

        gun.StatsChanged += OnStatsChanged;
    }

    private void UnsubscribeStatsEvents(IGun gun)
    {
        if (gun == null)
        {
            return;
        }

        gun.StatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(IWeaponStats stats)
    {
        _weaponChannel.Raise(new WeaponEvents.StatsChanged(stats));
    }

    private void SubscribeAmmoEvents(IAmmoEvents ammoEvents)
    {
        if (ammoEvents == null)
        {
            return;
        }

        ammoEvents.OnAmmoChange += _weaponChannel.RaiseAmmoChanged;
        ammoEvents.OnReloadStart += _weaponChannel.RaiseReloadStarted;
        ammoEvents.OnReloadEnd += _weaponChannel.RaiseReloadFinished;
    }

    private void UnsubscribeAmmoEvents(IAmmoEvents ammoEvents)
    {
        if (ammoEvents == null)
        {
            return;
        }

        ammoEvents.OnAmmoChange -= _weaponChannel.RaiseAmmoChanged;
        ammoEvents.OnReloadStart -= _weaponChannel.RaiseReloadStarted;
        ammoEvents.OnReloadEnd -= _weaponChannel.RaiseReloadFinished;
    }

    private void SwapAmmoEvents(IGun gun)
    {
        UnsubscribeAmmoEvents(_currentAmmoEvents);
        _currentAmmoEvents = gun.AmmoEvents;
        SubscribeAmmoEvents(_currentAmmoEvents);
    }
}

