using Systems.Guns.Projectiles;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

public sealed class SimpleProjectileWeapon : SimpleWeapon
{
    [SerializeField] private ProjectileConfigSO _projectileConfig;

    private ProjectileFactory _factory;

    private void Start()
    {
        _factory = ProjectileFactory.Instance;
    }

    protected override void Shoot(Vector3 position, Vector3 direction)
    {
        _factory
            .Create(_projectileConfig)
            .Shoot(position, direction);
    }
}