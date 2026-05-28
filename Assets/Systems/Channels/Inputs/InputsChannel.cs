using Systems.Channels.Inputs;
using Systems.Shared.Channels;
using UnityEngine;

namespace Systems.Channels
{
    [CreateAssetMenu(fileName = "InputEventChannel", menuName = MenuName + "InputChannel")]
    public sealed class InputsChannel : EventChannelBase<InputsChannel>
    {

        [ContextMenu("Sync Event Log Rules")]
        private void PopulateEventRules() => _logger.SyncEventLogRules();

        public void RaiseMove(Vector2 direction) => Raise(new InputEvents.Move(direction));
        public void RaiseLook(Vector2 direction) => Raise(new InputEvents.Look(direction));
        public void RaiseShoot(float value) => Raise(new InputEvents.Shoot(value > 0.9f));
        public void RaiseAbility(bool isPressed) => Raise(new InputEvents.RaiseAbility(isPressed));
        public void RaiseSelectWeapon(byte number) => Raise(new InputEvents.SelectWeapon(number));
        public void RaiseConfigureWeapon() => Raise(new InputEvents.ConfigureWeapon());

    }
}
