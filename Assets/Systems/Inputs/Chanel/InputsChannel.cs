using Systems.Shared.Channels;
using UnityEngine;

namespace Systems.Inputs.Channels
{
    [CreateAssetMenu(fileName = "InputEventChannel", menuName = MenuName + "InputChannel")]
    public sealed class InputsChannel : EventChannelBase<InputsChannel>
    {

        [ContextMenu("Sync Event Log Rules")]
        private void PopulateEventRules() => _logger.SyncEventLogRules();

        public void RaiseMove(Vector2 direction) => Raise(new InputEvents.Move(direction));
        public void RaiseShoot(float value) => Raise(new InputEvents.Shoot(value > 0.9f));
        public void RaiseAbility(bool isPressed) => Raise(new InputEvents.RaiseAbility(isPressed));
        public void RaiseSelectWeapon(byte number) => Raise(new InputEvents.SelectWeapon(number));
        public void RaiseConfigureWeapon() => Raise(new InputEvents.ConfigureWeapon());

        public void RaiseMouseDelta(Vector2 direction) => Raise(new InputEvents.MouseDelta(direction));
        public void RaiseMousePosition(Vector2 position) => Raise(new InputEvents.MousePosition(position));

    }
}
