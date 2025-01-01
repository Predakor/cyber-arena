using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {

    #region stats
    [Header("Weapon stats")]
    [SerializeField] float fireRate;
    [SerializeField] float magazineSize;
    [SerializeField] float currentAmmo;
    [SerializeField] float reloadSpeed;
    [SerializeField] float projectileSpeed;

    [Header("Weapon events")]
    Event onFire;
    Event onReload;
    Event onEmptyMagazine;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] Transform handTransform;
    AmmoDisplay ammoCountDisplay;

    [SerializeField] bool updateAmmoCounterUI = false;
    [SerializeField] bool isPlayersWeapon = false;
    float _fireRateCooldown = 0f;
    bool _isRealoading = false;
    #endregion

    void Start() {
        currentAmmo = magazineSize;
        _fireRateCooldown = Time.time;

        if (isPlayersWeapon || updateAmmoCounterUI) {
            ammoCountDisplay = AmmoDisplay.instance;
            ammoCountDisplay.SetAmmoText(currentAmmo);
        }

    }

    void LoadStats(GunData gunData) {
        fireRate = gunData.FireRate;
        magazineSize = gunData.MagazineSize;
        currentAmmo = gunData.CurrentAmmo;
        reloadSpeed = gunData.ReloadSpeed;
        projectileSpeed = gunData.ProjectileSpeed;
    }

    private void OnEnable() {
        if (ammoCountDisplay) {
            ammoCountDisplay.SetAmmoText(currentAmmo);
        }
        if (currentAmmo <= 0) {
            StartCoroutine(Reload());
        }
    }

    [ContextMenu("Bind to UI")]
    public void BindToUI() {
        updateAmmoCounterUI = true;
        isPlayersWeapon = true;
    }

    public void PickUp(GunData gunData) {
        WeaponManager.instance.PickupNewWeapon(gameObject);
        LoadStats(gunData);
    }

    [ContextMenu("Fire")]
    public void Fire() {
        if (_fireRateCooldown > Time.time || _isRealoading) {
            return;
        }

        if (currentAmmo <= 0) {
            StartCoroutine(Reload());
        }

        ShootProjectile();
        if (updateAmmoCounterUI || isPlayersWeapon) {
            ammoCountDisplay.SetAmmoText(currentAmmo);
        }

    }

    void ShootProjectile() {
        //to do use object pooling
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        Vector3 _dir = projectileSpawnPoint.forward * 20;
        Rigidbody _rb = projectile.GetComponent<Rigidbody>();
        _rb.AddForce(projectileSpeed * _dir);

        _fireRateCooldown = Time.time + (60 / fireRate);
        currentAmmo--;
    }

    IEnumerator Reload() {

        _isRealoading = true;

        yield return new WaitForSeconds(reloadSpeed);
        currentAmmo = magazineSize;
        _isRealoading = false;
        ammoCountDisplay.SetAmmoText(currentAmmo);

    }
}
