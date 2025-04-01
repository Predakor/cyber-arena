using System;
using UnityEngine;
using UnityEngine.Pool;

public class PhysicProjectileModule : ProjectileModule {

    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Projectile _projectile;

    ObjectPool<Projectile> _pool;

    public override Projectile Generate() {
        return _projectile = _pool.Get();
    }

    public override void AddImpactEffect() {
    }

    public override void Init() {
        if (_projectile == null) {
            Generate();
        }
        _projectile.enabled = true;
        _projectile.Init(DeInitProjectile, transform.position, 20);
    }

    void Awake() {
        _pool = new(
            InstantiateProjectile,
            InitProjectile,
            DeInitProjectile,
            DestroyProjectile,
            false,
            _poolSize,
            _poolMaxSize
        );
    }

    Action<IDamageable> HandleImpact() {
        return damagable => damagable.Damage(10);
    }

    Projectile InstantiateProjectile() {
        return _projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }

    void InitProjectile(Projectile projectile) {
        _projectile.OnDamageableHit += HandleImpact();
        projectile.enabled = true;
    }

    void DeInitProjectile(Projectile projectile) {
        _projectile.OnDamageableHit -= HandleImpact();
        projectile.enabled = false;
    }

    void DestroyProjectile(Projectile projectile) {
        Destroy(projectile.gameObject);
    }
}
