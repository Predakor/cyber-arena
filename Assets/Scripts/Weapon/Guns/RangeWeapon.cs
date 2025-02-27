using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RangeWeapon : Weapon {
    [Header("Weapon stats")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float magazineSize = 5f;
    [SerializeField] float currentAmmo = 5f;
    [SerializeField] float reloadSpeed = 0.5f;

    [Header("Weapon events")]
    public UnityEvent onFire;
    public UnityEvent onReloadStart;
    public UnityEvent onReloadEnd;
    public UnityEvent onEmptyMagazine;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileSpeed = 10f;

    protected float _fireRateCooldown = 0f;
    protected bool _isReloading = false;

    public float FireRate { get => fireRate; }
    public float MagazineSize { get => magazineSize; }
    public float CurrentAmmo { get => currentAmmo; }
    public float ReloadSpeed { get => reloadSpeed; }

    #region helpers
    protected override void LoadStats(GunData gunData) {
        fireRate = gunData.FireRate;
        magazineSize = gunData.MagazineSize;
        currentAmmo = gunData.CurrentAmmo;
        reloadSpeed = gunData.ReloadSpeed;
        projectileSpeed = gunData.ProjectileSpeed;
    }

    #endregion


    void Awake() {
    }

    void Start() {
        _fireRateCooldown = Time.time;
    }

    private void OnEnable() {
        if (CurrentAmmo <= 0) {
            StartCoroutine(Reload());
        }
    }

    public void Inspect() {
        throw new System.NotImplementedException();
    }

    public override void Fire() {
        if (_fireRateCooldown > Time.time || _isReloading) {
            return;
        }

        if (currentAmmo < 1) {
            StartCoroutine(Reload());
            return;
        }

        ShootProjectile();
    }

    [ContextMenu("Fire")]
    void ShootProjectile(GameObject _projectile = null) {
        currentAmmo -= 1;

        Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Projectile>();

        projectile.Initialize(projectileSpawnPoint, projectileSpeed, 10f);

        _fireRateCooldown = Time.time + (60 / FireRate);

        onFire?.Invoke();

        if (currentAmmo < 1) {
            onEmptyMagazine?.Invoke();
        }
    }

    IEnumerator Reload() {
        if (currentAmmo >= magazineSize || _isReloading) {
            yield break;
        }

        onReloadStart?.Invoke();
        _isReloading = true;

        yield return new WaitForSeconds(ReloadSpeed);

        onReloadEnd?.Invoke();
        currentAmmo = MagazineSize;
        _isReloading = false;

    }
}
