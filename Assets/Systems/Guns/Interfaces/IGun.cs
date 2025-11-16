using Systems.Guns.Interfaces;

namespace Systems.Guns {
    public interface IWeapon {
        TWeapon Configure<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : IWeapon;
        void Use();
    };

    public interface IGun : IWeapon { }

    public interface IMelee : IWeapon { }

    public interface IShield : IWeapon { }
}
