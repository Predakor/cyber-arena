using System;
using UnityEngine;
using UnityEngine.Pool;

public class PhysicProjectileModule : ProjectileModule {

    [SerializeField] Projectile _projectilePrefab;

    [SerializeField] float _speed;
    ObjectPool<Projectile> _pool;

    public override Projectile Get() {
        return _pool.Get();
    }

    public override void AddImpactEffect() {
    }

    public override void Init(GunData data) {
        _speed = data.ProjectileSpeed;
        _poolSize = data.MagazineSize;
        _poolMaxSize = 2 * _poolSize;

        _pool = new(
            CreateProjectile,
            OnGetProjectile,
            OnReleaseProjectile,
            DestroyProjectile,
            false,
            _poolSize,
            _poolMaxSize
        );
    }

    Action<IDamageable> HandleImpact() {
        return damagable => damagable.Damage(10);
    }

    Projectile CreateProjectile() {
        Projectile projectile = Instantiate(_projectilePrefab);
        projectile.Init((p) =>
        _pool.Release(p), _speed);
        return projectile;
    }

    void OnGetProjectile(Projectile projectile) {
        projectile.gameObject.SetActive(true);
        projectile.OnDamageableHit += HandleImpact();
    }

    void OnReleaseProjectile(Projectile projectile) {
        projectile.gameObject.SetActive(false);
        projectile.OnDamageableHit -= HandleImpact();
    }

    void DestroyProjectile(Projectile projectile) {
        Destroy(projectile.gameObject);
    }
}
