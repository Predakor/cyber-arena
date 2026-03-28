using System;
using UnityEngine;
using UnityEngine.Pool;

public class PhysicProjectileModule : ProjectileModule
{

    [SerializeField] private Projectile _projectilePrefab;

    [SerializeField] private float _speed;
    private ObjectPool<Projectile> _pool;

    public override Projectile Get()
    {
        return _pool.Get();
    }

    public override void AddImpactEffect()
    {
    }

    public override void Init(GunData data)
    {
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

    private Action<IDamageable> HandleImpact()
    {
        return damagable => damagable.Damage(10);
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(_projectilePrefab);
        projectile.Init((p) =>
        _pool.Release(p), _speed);
        return projectile;
    }

    private void OnGetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.OnDamageableHit += HandleImpact();
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.OnDamageableHit -= HandleImpact();
    }

    private void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
