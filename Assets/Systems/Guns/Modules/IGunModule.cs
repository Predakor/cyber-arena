using System;
using Systems.Guns;
using Systems.Guns.Modules.Shared;

namespace Systems.Weapons.Guns.Modules
{
    public interface IGunModule
    {
        void Handle(ShootContext context, Action<ShootContext> next);
        void Apply(WeaponStatsBuilder stats);
    }
}
