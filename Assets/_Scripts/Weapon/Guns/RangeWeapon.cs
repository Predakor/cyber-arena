using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RangeWeapon : Weapon {
    [Header("Weapon stats")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] int magazineSize = 5;
    [SerializeField] int currentAmmo = 5;
    [SerializeField] float reloadSpeed = 0.5f;

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

    public float FireRate { get => fireRate; }
    public int MagazineSize { get => magazineSize; }
    public int CurrentAmmo {
        get => currentAmmo; private set {
            currentAmmo = value;
            onAmmoChange?.Invoke((int)value);

            if (currentAmmo < 1) onEmptyMagazine?.Invoke();

        }
    }

    public float ReloadSpeed { get => reloadSpeed; }

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
    [ContextMenu("Start Reload")]
    public void StartReload() {
        _reloadCoroutine = StartCoroutine(Reload());
    }

    [ContextMenu("End Reload")]
    public void StopReload() {
        StopCoroutine(_reloadCoroutine);
    }

    [ContextMenu("Fire")]
    public override void Fire() {
        if (_fireRateCooldown > Time.time || _isReloading) {
            return;
        }

        if (currentAmmo < 1) {
            StartReload();
            return;
        }

        ShootProjectile();
    }
    #endregion

    void Start() {
        _fireRateCooldown = Time.time;
    }

    void OnEnable() {
        if (CurrentAmmo <= 0) {
            StartCoroutine(Reload());
        }
    }
    void OnDisable() {
        StopAllCoroutines();
    }

    public void Inspect() {
        throw new System.NotImplementedException();
    }

    void ShootProjectile(GameObject _projectile = null) {
        CurrentAmmo--;

        GameObject overrideProjectile = _projectile == null ? projectilePrefab : _projectile;

        Projectile projectile = Instantiate(overrideProjectile, transform.position, transform.rotation).GetComponent<Projectile>();

        projectile.Initialize(projectileSpawnPoint, projectileSpeed, 10f);

        _fireRateCooldown = Time.time + (60 / FireRate);

        onFire?.Invoke();
    }

    IEnumerator Reload() {
        if (currentAmmo >= magazineSize || _isReloading) {
            yield break;
        }

        onReloadStart?.Invoke();
        _isReloading = true;

        yield return new WaitForSeconds(ReloadSpeed);

        onReloadEnd?.Invoke();
        CurrentAmmo = MagazineSize;
        _isReloading = false;

    }
}
