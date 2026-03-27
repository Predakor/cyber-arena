using System;
using System.Collections;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

[Serializable]
public abstract class AmmoModule : MonoBehaviour, IGunModule
{
    [SerializeField] private int _magazineSize;
    [SerializeField] private int _currentAmmo;
    [Range(0f, 10f)]
    [SerializeField] private float _reloadSpeed;

    public event Action OnReload;
    public event Action OnReloadEnd;
    public event Action<int> OnAmmoChange;

    private bool _isReloading = false;

    public virtual int CurrentAmmo
    {
        get => _currentAmmo; protected set
        {
            if (_currentAmmo == value)
            {
                return;
            }
            _currentAmmo = value;
            OnAmmoChange(value);
        }
    }

    public virtual int MagazineSize
    {
        get => _magazineSize; protected set
        {
            _magazineSize = value;
        }
    }

    public virtual float ReloadSpeed
    {
        get => _reloadSpeed; protected set
        {
            _reloadSpeed = value;
        }
    }

    public virtual void Init(GunData data)
    {
        _currentAmmo = data.CurrentAmmo;
        _magazineSize = data.MagazineSize;
        _reloadSpeed = data.ReloadSpeed;
    }


    public abstract IEnumerator Reload();
    public abstract void DecreaseAmmo(int amount = 1);
    public abstract void IncreaseAmmo(int amount = 1);

    protected virtual void StartReload()
    {
        if (_isReloading)
        {
            return;
        }
        _isReloading = true;
        OnReload?.Invoke();
    }

    protected virtual void FinishReload()
    {
        _isReloading = false;
        CurrentAmmo = MagazineSize;
        OnReloadEnd?.Invoke();
    }

    public void Handle(ShootContext context, Action<ShootContext> next)
    {
        if (_isReloading)
        {
            return;
        }

        if (context.AmmoCost > CurrentAmmo)
        {
            StartReload();
        }

        DecreaseAmmo(context.AmmoCost);

        next(context);
    }


}
