using System;

namespace Systems.Weapons.Guns.Modules
{
    public interface IAmmoEvents
    {
        /// <summary>current, max</summary>
        event Action<int, int> OnAmmoChange;
        /// <summary>duration</summary>
        event Action<float> OnReloadStart;
        /// <summary>duration, interrupted</summary>
        event Action<float, bool> OnReloadEnd;
    }
}