using Systems.Channels.Inputs;
using UnityEngine;

namespace Systems.Channels {
    [CreateAssetMenu(fileName = "InputEventChannel", menuName = MenuName + "/InputChannel")]
    public sealed class InputsChannel : EventChannelBase {
        public void RaiseMove(Vector2 direction) => Raise(new InputEvents.Move(direction));

        public void RaiseLook(Vector2 direction) => Raise(new InputEvents.Look(direction));

        public void RaiseShoot(float value) => Raise(new InputEvents.Shoot(value > 0.9f));

        public void RaiseAbility(bool isPressed) => Raise(new InputEvents.RaiseAbility(isPressed));
    }
}
