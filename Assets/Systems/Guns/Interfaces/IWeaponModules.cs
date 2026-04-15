using Systems.Weapons.Guns.Modules;

namespace Systems.Guns.Assets.Systems.Guns.Interfaces
{
    public interface IWeaponModules
    {
        IGunModule FireRateModule { get; }
        IGunModule AmmoModule { get; }
        IGunModule SpreadModule { get; }
    }
}