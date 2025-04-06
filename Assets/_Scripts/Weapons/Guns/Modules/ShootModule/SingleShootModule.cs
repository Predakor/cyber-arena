using UnityEngine;

public class SingleShootModule : ShootModule {
    [SerializeField] Transform _bulletStart;

    public override void Shoot(Projectile projectile) {
        projectile.Reuse(_bulletStart.position, _bulletStart.rotation);
        projectile.enabled = true;
    }

}
