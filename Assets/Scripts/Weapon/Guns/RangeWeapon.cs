using System.Collections;
using UnityEngine;

public class RangeWeapon : Weapon {
    [Header("Weapon stats")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float magazineSize = 5f;
    [SerializeField] float currentAmmo = 5f;
    [SerializeField] float reloadSpeed = 0.5f;


    [Header("Weapon events")]
    Event onFire;
    Event onReload;
    Event onEmptyMagazine;


    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileSpeed = 1f;

    float _fireRateCooldown = 0f;
    bool _isRealoading = false;

    public float FireRate { get => fireRate; }
    public float MagazineSize { get => magazineSize; }
    public float CurrentAmmo { get => currentAmmo; }
    public float ReloadSpeed { get => reloadSpeed; }


    protected override void LoadStats(GunData gunData) {
        fireRate = gunData.FireRate;
        magazineSize = gunData.MagazineSize;
        currentAmmo = gunData.CurrentAmmo;
        reloadSpeed = gunData.ReloadSpeed;
        projectileSpeed = gunData.ProjectileSpeed;
    }

    void Start() {
        currentAmmo = magazineSize;
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

    [ContextMenu("Fire")]
    public override void Fire() {

        if (_fireRateCooldown > Time.time || _isRealoading) {
            return;
        }

        if (currentAmmo < 1) {
            StartCoroutine(Reload());
        }

        ShootProjectile();
    }

    void ShootProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
        _fireRateCooldown = Time.time + (60 / FireRate);
        currentAmmo -= 1;
    }

    IEnumerator Reload() {

        _isRealoading = true;

        yield return new WaitForSeconds(ReloadSpeed);
        currentAmmo = MagazineSize;
        _isRealoading = false;
    }
}
