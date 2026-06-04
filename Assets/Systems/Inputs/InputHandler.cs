using Systems.Channels;
using Systems.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Systems.Inputs
{
    public sealed class InputsHandler : Singleton<InputsHandler>, PlayerControls.IPlayerActions
    {
        [SerializeField] private InputsChannel _channel;

        private PlayerControls _controls;

        protected override void Awake()
        {
            base.Awake();
            _controls = new PlayerControls();
            _controls.Player.SetCallbacks(this);
        }

        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => _controls.Player.Disable();
        public void Enable(bool state) => gameObject.SetActive(state);

        // The generated interface forces you to implement these specific methods
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _channel.RaiseMove(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                _channel.RaiseMove(Vector2.zero);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _channel.RaiseLook(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                _channel.RaiseLook(Vector2.zero);
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _channel.RaiseShoot(context.ReadValue<float>());
            }
            else if (context.canceled)
            {
                _channel.RaiseShoot(0);
            }
        }

        public void OnDash(InputAction.CallbackContext context) => _channel.RaiseAbility(context.performed);
        public void OnSelectWeapon(InputAction.CallbackContext context) => _channel.RaiseSelectWeapon(context.ToNumber());
        public void OnConfigureWeapon(InputAction.CallbackContext context) => _channel.RaiseConfigureWeapon();

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }


    }

    internal static class InputCallbackExtensions
    {
        public static byte ToNumber(this InputAction.CallbackContext ct)
        {
            // Note: Ensure your Action is bound to keys that support this
            var keyControl = ct.control as KeyControl;
            return (byte)(keyControl.keyCode - Key.Digit1);
        }
    }
}