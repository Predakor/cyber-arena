using UnityEngine;
using UnityEngine.Events;

public interface IGun {
    public void Shoot();
    public void Reload();
}

[RequireComponent(typeof(AmmoModule), typeof(ShootModule), typeof(ProjectileModule))]
public abstract class Gun : Weapon {

    [SerializeField] protected GunData gunData;
    [SerializeField] protected AmmoModule _ammoModule;
    [SerializeField] protected ShootModule _shootModule;
    [SerializeField] protected ProjectileModule _projectileModule;

    [Header("Weapon stats")]
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected int magazineSize = 5;
    [SerializeField] protected int currentAmmo = 5;
    [SerializeField] protected float reloadSpeed = 0.5f;
    [SerializeField] protected GunState _state = GunState.Holster;

    [Header("Weapon events")]
    public UnityEvent onFire;
    public UnityEvent onReloadStart;
    public UnityEvent onReloadEnd;
    public UnityEvent onEmptyMagazine;
    public UnityEvent<int> onAmmoChange;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileSpeed = 10f;

    protected float _fireRateCooldown = 0f;
    protected bool _isReloading = false;
    protected Coroutine _reloadCoroutine;

    public int CurrentAmmo {
        get => _ammoModule.CurrentAmmo; private set {
            currentAmmo = value;
            onAmmoChange?.Invoke(value);

            if (currentAmmo < 1) onEmptyMagazine?.Invoke();

        }
    }

    #region helpers
    public override void LoadStats(GunData gunData) {
        fireRate = gunData.FireRate;
        magazineSize = gunData.MagazineSize;
        currentAmmo = gunData.CurrentAmmo;
        reloadSpeed = gunData.ReloadSpeed;
        projectileSpeed = gunData.ProjectileSpeed;
    }

    #endregion

    #region public methods

    [ContextMenu("Load Data")]
    public void LoadData() => LoadStats(gunData);
    public void Init(GunData data) {
        LoadStats(data);
        _ammoModule.OnReload += () => _state = GunState.Reloading;
        _ammoModule.OnReloadEnd += () => _state = GunState.Ready;
        _ammoModule.OnAmmoChange += (amount) => CurrentAmmo = amount;

    }

    [ContextMenu("Start Reload")]
    abstract public void Reload();

    [ContextMenu("Fire")]
    abstract public void Fire();

    public void Inspect() {
        throw new System.NotImplementedException();
    }

    #endregion

    protected virtual void Start() {
        _fireRateCooldown = Time.time;
    }

    protected virtual void OnEnable() {
        if (CurrentAmmo < 1) {
            Reload();
        }
    }

    protected virtual void OnDisable() {
        StopAllCoroutines();
    }

    virtual protected void OnValidate() {
        if (gunData) {
            LoadData();
        }
    }
}

public enum GunState {
    Ready,      // can shoot 
    Aiming,     // can shot and gets extra buufs
    Holster,    // can't shoot need to wait for holster animation to finish
    Shooting,   // can shoot but is shooting
    Reloading,  // can't shoot need to wait for reload to end
}