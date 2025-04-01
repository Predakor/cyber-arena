public class SingleShootModule : ShootModule {
    public override void Shoot() {
        _projectile.Fire();
    }

    public override void Shoot(Projectile projectile) {
        projectile.Fire();
    }

}
