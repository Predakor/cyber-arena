using UnityEngine;

namespace Systems.Channels.Inputs
{
    public static class InputEvents
    {
        public sealed record Move(Vector2 Direction);
        public sealed record Look(Vector2 Direction);
        public sealed record Shoot(bool IsPressed);
        public sealed record RaiseAbility(bool IsPressed);
        public sealed record SelectWeapon(byte WeaponNumber);
        public sealed record ConfigureWeapon();
    }

}
