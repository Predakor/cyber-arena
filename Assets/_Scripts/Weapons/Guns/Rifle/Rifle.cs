using UnityEngine;

public class Rifle : Gun {

    [SerializeField] Vector3 _spread;
    float _nextShootCooldown;

    public override void LoadStats(GunData data) {
#if UNITY_EDITOR
        if (data is not RifleDataSo) {
            Debug.LogWarning("Wrong GunTypeData", this);
        }
#endif
        RifleDataSo rifleDataSo = data as RifleDataSo;
        _spread = rifleDataSo.Spread;
        _nextShootCooldown = 1 / data.FireRate;
        base.LoadStats(rifleDataSo);

        _ammoModule.Init(data);
        _projectileModule.Init(data);
    }

    [ContextMenu("Fire")]
    public override void Fire() {
        float time = Time.time;
        bool canShoot = _lastShootTime < time && !_isReloading;

        if (!canShoot) {
            return;
        }

        if (_state == GunState.Holster) {
            //wait for holster animation to end
            //if holster animation not started start it 
        }

        if (_currentAmmo < 1) {
            Reload();
            return;
        }

        ShootProjectile();
        _lastShootTime = time + _nextShootCooldown;
    }


    [ContextMenu("Start Reload")]
    public override void Reload() {
        StartCoroutine(_ammoModule.Reload());
    }
    void ShootProjectile() {
        Projectile projectile = _projectileModule.Get();

        if (_ammoModule.CurrentAmmo > 0) {
            _shootModule.Shoot(projectile);
            _ammoModule.DecreaseAmmo();
            //_soundModule.Play("Shoot Sound");
        }
    }
}
