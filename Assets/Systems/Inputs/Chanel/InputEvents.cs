using Systems.Shared.Channels;
using UnityEngine;

namespace Systems.Inputs.Channels
{
    [ChannelEvents(typeof(InputsChannel))]
    public static class InputEvents
    {
        public sealed record Move(Vector2 Direction);
        public sealed record Shoot(bool IsPressed);
        public sealed record RaiseAbility(bool IsPressed);
        public sealed record SelectWeapon(byte WeaponNumber);
        public sealed record ConfigureWeapon();

        public readonly struct MousePosition
        {
            public readonly Vector2 Position;
            public MousePosition(Vector2 position) => Position = position;
        }

        public readonly struct MouseDelta
        {
            public readonly Vector2 Direction;
            public MouseDelta(Vector2 direction) => Direction = direction;
        }
    }

}
