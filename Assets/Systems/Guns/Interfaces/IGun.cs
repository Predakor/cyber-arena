using System;
using Systems.Guns.Assets.Systems.Guns.Interfaces;
using Systems.Guns.Interfaces;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns
{
    public interface IWeapon
    {
        TWeapon Configure<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : IWeapon;
        void Use(bool isPressed);
    };

    public interface IGun : IWeapon
    {
        IAmmoEvents AmmoEvents { get; }
        IWeaponStats Stats { get; }
        IWeaponModules Modules { get; }

        event Action<IWeaponStats> StatsChanged;
        event Action<IWeaponModules> ModulesChanged;
    }


    public interface IMelee : IWeapon { }

    public interface IShield : IWeapon { }
}
