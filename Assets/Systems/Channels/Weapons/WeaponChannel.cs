using UnityEngine;

namespace Systems.Channels.Weapons
{
    [CreateAssetMenu(fileName = "WeaponChannel", menuName = MenuName + "WeaponChannel")]
    public sealed class WeaponChannel : EventChannelBase
    {
        public void RaiseSelected(byte slot, byte? prevSlot = null) => Raise(new WeaponEvents.Selected(slot, prevSlot));
        public void RaiseFired() => Raise(new WeaponEvents.Fired());
        public void RaiseAmmoChanged(int current, int? reserve = null) => Raise(new WeaponEvents.AmmoChanged(current, reserve));
        public void RaiseReloadStarted(float duration) => Raise(new WeaponEvents.ReloadStarted(duration));
        public void RaiseReloadFinished() => Raise(new WeaponEvents.ReloadFinished());
    }
}
