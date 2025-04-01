using UnityEngine;

public class Rifle : Gun {
    public override void Fire() {
        bool isReadyToShoot = _fireRateCooldown < Time.time || !_isReloading;

        if (isReadyToShoot) {
            return;
        }

        if (currentAmmo < 1) {
            Reload();
            return;
        }

        ShootProjectile();
    }

    public override void Reload() {
        StartCoroutine(_ammoModule.Reload());
    }


    void ShootProjectile() {
        Projectile projectile = _projectileModule.Generate();
        if (_state == GunState.Reloading) {
            return;
        }

        if (_state == GunState.Holster) {
            //wait for holster animation to end
        }

        if (_ammoModule.CurrentAmmo > 0) {
            _shootModule.Shoot(projectile);
            _ammoModule.DecreaseAmmo();
            //_soundModule.Play("Shoot Sound");
        }
    }
}
