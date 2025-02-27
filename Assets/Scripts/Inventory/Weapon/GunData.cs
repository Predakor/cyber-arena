using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class GunData : ItemData {
    [SerializeField] float fireRate = 1f;
    [SerializeField] int magazineSize = 5;
    [SerializeField] int currentAmmo = 5;
    [SerializeField] float reloadSpeed = 0.5f;
    [SerializeField] float projectileSpeed = 1f;

    public float FireRate => fireRate;
    public int MagazineSize => magazineSize;
    public int CurrentAmmo => currentAmmo;
    public float ReloadSpeed => reloadSpeed;
    public float ProjectileSpeed => projectileSpeed;
}
