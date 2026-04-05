using System;
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
        event Action<IWeaponStats> StatsChanged;
    }


    public interface IMelee : IWeapon { }

    public interface IShield : IWeapon { }
}
