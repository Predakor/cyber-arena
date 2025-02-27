public class GunInstance : ItemInstance {
    public float FireRate { get; private set; }
    public float MagazineSize { get; private set; }
    public float CurrentAmmo { get; private set; }
    public float ReloadSpeed { get; private set; }
    public float ProjectileSpeed { get; private set; }

    public GunInstance(GunData itemData) : base(itemData) {
        FireRate = itemData.FireRate;
        MagazineSize = itemData.MagazineSize;
        CurrentAmmo = itemData.CurrentAmmo;
        ReloadSpeed = itemData.ReloadSpeed;
        ProjectileSpeed = itemData.ProjectileSpeed;
    }
}
