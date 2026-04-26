using System;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns.Modules
{
    public sealed class ProjectileSpawnModule : IGunModule
    {
        public string Name { get; set; } = "Projectile Spawner";

        public void Handle(ShootContext context, Action<ShootContext> next)
        {
            var shots = context.ShotPoints;
            if (shots == null || shots.Length == 0)
            {
                shots = new[] { new ShotPoint(context.Muzzle.position, context.Muzzle.forward) };
            }

            foreach (var shot in shots)
            {
                ProjectileFactory.Instance
                    .Create(context.ProjectileConfig)
                    .Apply(context)
                    .Shoot(shot.Origin, shot.Direction);
            }
        }

        public void Apply(WeaponStatsBuilder stats)
        {
            throw new NotImplementedException();
        }
    }
}