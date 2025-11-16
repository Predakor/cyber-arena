using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;

namespace Systems.Guns.Projectiles {
    public interface IProjectile : IShootable, IConfigurable<IProjectileConfig> { }

    public interface IShootable {
        void Shoot();
    }

    public interface IProjectileConfig : IConfig {
        float Size { get; }
        int Damage { get; }
        float Speed { get; }

        IReadOnlyList<IHitEffect> Effects { get; }
    }
}
