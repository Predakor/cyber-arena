using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class GunData : ItemData {
    [SerializeField] float fireRate = 1f;
    [SerializeField] float magazineSize = 5f;
    [SerializeField] float currentAmmo = 5f;
    [SerializeField] float reloadSpeed = 0.5f;
    [SerializeField] float projectileSpeed = 1f;

    public float FireRate => fireRate;
    public float MagazineSize => magazineSize;
    public float CurrentAmmo => currentAmmo;
    public float ReloadSpeed => reloadSpeed;
    public float ProjectileSpeed => projectileSpeed;
}
