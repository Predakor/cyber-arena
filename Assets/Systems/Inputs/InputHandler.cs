using Systems.Channels;
using Systems.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Systems.Inputs {
    public sealed class InputsHandler : Singleton<InputsHandler> {
        [SerializeField]
        private InputActionAsset playerControls;

        [SerializeField]
        private InputsChannel _channel;

        [SerializeField]
        private string actionMapName = "Player";

        [Header("Action Name References")]
        [SerializeField]
        private string move = "Move";

        [SerializeField]
        private string look = "Look";

        [SerializeField]
        private string shoot = "Shoot";

        [SerializeField]
        private string dash = "Dash";

        [SerializeField]
        string selectWeapon = "SelectWeapon";

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction shootAction;
        private InputAction dashAction;
        private InputAction selectWeaponAction;

        private void Start() {
            var actionMap = playerControls.FindActionMap(actionMapName);

            moveAction = actionMap.FindAction(move);
            lookAction = actionMap.FindAction(look);
            shootAction = actionMap.FindAction(shoot);
            dashAction = actionMap.FindAction(dash);
            selectWeaponAction = actionMap.FindAction(selectWeapon);

            RegisterInputActions();
            gameObject.SetActive(true);
        }

        void RegisterInputActions() {
            moveAction.performed += ct => _channel.RaiseMove(ct.ReadValue<Vector2>());
            moveAction.canceled += _ => _channel.RaiseMove(Vector2.zero);

            lookAction.performed += ct => _channel.RaiseLook(ct.ReadValue<Vector2>());
            lookAction.canceled += _ => _channel.RaiseLook(Vector2.zero);

            shootAction.performed += ct => _channel.RaiseShoot(ct.ReadValue<float>());
            shootAction.canceled += _ => _channel.RaiseShoot(0);

            dashAction.performed += _ => _channel.RaiseAbility(true);
            dashAction.canceled += _ => _channel.RaiseAbility(false);

            selectWeaponAction.performed += ct => _channel.RaiseSelectWeapon(ct.ToNumber());
        }

        void SetActionMapEnabled(bool enabled) {
            var actions = new[]
            {
                moveAction,
                lookAction,
                shootAction,
                dashAction,
                selectWeaponAction,
            };

            foreach (var action in actions) {
                if (enabled) {
                    action.Enable();
                }
                else {
                    action.Disable();
                }
            }
        }

        void OnEnable() => SetActionMapEnabled(true);

        void OnDisable() => SetActionMapEnabled(false);
    }

    internal static class InputCallbacExtentions {
        public static byte ToNumber(this InputAction.CallbackContext ct) {
            var keyControll = ct.control as KeyControl;
            return (byte)(keyControll.keyCode - Key.Digit1);
        }
    }
}
