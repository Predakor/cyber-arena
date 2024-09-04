using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable, IInstpectable {


    [Header("Weapon stats")]
    [SerializeField] float fireRate = 1f;

    [SerializeField] float magazineSize = 5f;
    [SerializeField] float currentAmmo = 5f;
    [SerializeField] float reloadSpeed = 0.5f;

    [SerializeField] float projectileSpeed = 1f;

    [Header("Weapon events")]
    Event onFire;
    Event onReload;
    Event onEmptyMagazine;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    AmmoDisplay ammoCountDisplay;

    float _fireRateCooldown = 0f;
    bool _isRealoading = false;

    void Start() {
        currentAmmo = magazineSize;
        _fireRateCooldown = Time.time;
        ammoCountDisplay = AmmoDisplay.instance;
        ammoCountDisplay.SetAmmoText(currentAmmo);
    }

    private void OnEnable() {
        if (ammoCountDisplay) {
            ammoCountDisplay.SetAmmoText(currentAmmo);
        }
        if (currentAmmo <= 0) {
            StartCoroutine(Reload());
        }
    }

    public void PickUp() {
        WeaponManager.instance.PickupNewWeapon(gameObject);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void Inspect() {
        throw new System.NotImplementedException();
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
        ammoCountDisplay.SetAmmoText(currentAmmo);

    }

    void ShootProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
        _fireRateCooldown += 60 / fireRate;
        currentAmmo -= 1;
    }

    IEnumerator Reload() {

        _isRealoading = true;

        yield return new WaitForSeconds(reloadSpeed);
        currentAmmo = magazineSize;
        _isRealoading = false;
        ammoCountDisplay.SetAmmoText(currentAmmo);

    }
}
