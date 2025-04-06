using System;
using UnityEngine;
using UnityEngine.Events;


public abstract class Gun : Weapon {
    [SerializeField] protected GunData _gunData;
    [SerializeField] protected AmmoModule _ammoModule;
    [SerializeField] protected ShootModule _shootModule;
    [SerializeField] protected ProjectileModule _projectileModule;

    [Header("Weapon stats")]
    [SerializeField] protected float _fireRate = 1f;
    [SerializeField] protected int _magazineSize = 5;
    [SerializeField] protected int _currentAmmo = 5;
    [SerializeField] protected float _reloadSpeed = 0.5f;
    [SerializeField] protected GunState _state = GunState.Holster;

    [Header("Weapon events")]
    public UnityEvent onFire;
    public UnityEvent onReloadStart;
    public UnityEvent onReloadEnd;
    public UnityEvent<int> onAmmoChange;

    protected float _lastShootTime = 0f;
    protected bool _isReloading = false;
    protected Coroutine _reloadCoroutine;


    public float FireRate { get => _fireRate; }
    public int MagazineSize { get => _magazineSize; }
    public int CurrentAmmo {
        get => _ammoModule.CurrentAmmo; private set {
            _currentAmmo = value;
            onAmmoChange?.Invoke(value);
        }
    }
    public float ReloadSpeed { get => _reloadSpeed; }
    public GunState State { get => _state; }


    #region public methods
    public virtual void Init(GunData data) {
        LoadStats(data);
        ClearEvents();
        SetupEvents();
    }
    public override void LoadStats(GunData gunData) {
        _fireRate = gunData.FireRate;
        _magazineSize = gunData.MagazineSize;
        _currentAmmo = gunData.CurrentAmmo;
        _reloadSpeed = gunData.ReloadSpeed;
    }

    abstract public void Reload();

    abstract public void Fire();

    #endregion

    #region lifetime methods

    protected virtual void OnEnable() {
        _lastShootTime = Time.time;
        SetupEvents();
        if (CurrentAmmo < 1) {
            Reload();
        }
    }
    protected virtual void OnDisable() {
        ClearEvents();
    }
    protected virtual void OnValidate() {
        if (_gunData) {
            LoadStats(_gunData);
        }
    }

    #endregion

    protected virtual void SetupEvents() {
        _ammoModule.OnReload += HandleReload();
        _ammoModule.OnReloadEnd += HandleReloadEnd();
        _ammoModule.OnAmmoChange += HandleAmmoChange();
    }

    protected virtual void ClearEvents() {
        _ammoModule.OnReload -= HandleReload();
        _ammoModule.OnReloadEnd -= HandleReloadEnd();
        _ammoModule.OnAmmoChange -= HandleAmmoChange();
    }

    protected virtual Action HandleReload() => () => _state = GunState.Reloading;
    protected virtual Action HandleReloadEnd() => () => _state = GunState.Ready;
    protected virtual Action<int> HandleAmmoChange() => (amount) => CurrentAmmo = amount;
}

public enum GunState {
    Ready,      // can shoot 
    Aiming,     // can shot and gets extra buufs
    Holster,    // can't shoot need to wait for holster animation to finish
    Shooting,   // can shoot but is shooting
    Reloading,  // can't shoot need to wait for reload to end
}