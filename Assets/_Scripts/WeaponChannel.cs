using Systems.Guns;
using Systems.Guns.Assets.Systems.Guns.Interfaces;
using UnityEngine;

namespace Systems.Channels.Weapons
{
    [CreateAssetMenu(fileName = "WeaponChannel", menuName = MenuName + "WeaponChannel")]
    public sealed class WeaponChannel : EventChannelBase
    {
        public void RaiseSelected(byte slot, byte? prevSlot = null) => Raise(new WeaponEvents.Selected(slot, prevSlot));
        public void RaiseFired() => Raise(new WeaponEvents.Fired());
        public void RaiseAmmoChanged(int current) => Raise(new WeaponEvents.AmmoChanged(current));
        public void RaiseAmmoChanged(int current, int reserve) => Raise(new WeaponEvents.AmmoChanged(current, reserve));

        public void RaiseReloadStarted(float duration) => Raise(new WeaponEvents.ReloadStarted(duration));
        public void RaiseReloadFinished(float duration, bool hasBeenInterupted = false) => Raise(new WeaponEvents.ReloadFinished(duration, hasBeenInterupted));

        public void RaiseStatsChanged(IWeaponStats stats) => Raise(new WeaponEvents.StatsChanged(stats));
        public void RaiseModulesChanged(IWeaponModules modules) => Raise(new WeaponEvents.ModulesChanged(modules));

        public void RaiseReconfigured(Configuration config) => Raise(new WeaponEvents.Reconfigured(config));
    }
}
