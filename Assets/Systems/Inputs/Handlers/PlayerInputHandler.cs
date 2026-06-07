using System;
using System.Collections.Generic;
using Systems.Inputs.Channels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Inputs.Handlers
{
    internal sealed class PlayerInputHandler
        : InputHandlerBase, PlayerControls.IPlayerActions
    {
        private const float UpdateInterval = 0.05f; // 50 ms
        private float _lastDeltaUpdateTime;
        private float _lastPositionUpdateTime;

        public PlayerInputHandler(InputsChannel channel) : base(channel) { }

        public override void Init(PlayerControls controls, Dictionary<ControlType, Action> enablers, List<InputActionMap> actionMaps)
        {
            controls.Player.SetCallbacks(this);
            enablers.Add(ControlType.Gameplay, () => controls.Player.Enable());
            actionMaps.Add(controls.Player);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Channel.RaiseMove(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                Channel.RaiseMove(Vector2.zero);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            float currentTime = Time.time;
            bool intervalPassed = currentTime - _lastDeltaUpdateTime >= UpdateInterval;
            if (!intervalPassed)
            {
                return;
            }

            _lastDeltaUpdateTime = currentTime;
            if (context.performed)
            {
                Channel.RaiseMouseDelta(context.ReadValue<Vector2>());
            }

        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Channel.RaiseShoot(context.ReadValue<float>());
            }
            else if (context.canceled)
            {
                Channel.RaiseShoot(0);
            }
        }

        public void OnDash(InputAction.CallbackContext context) => Channel.RaiseAbility(context.performed);
        public void OnSelectWeapon(InputAction.CallbackContext context) => Channel.RaiseSelectWeapon(context.ToNumber());
        public void OnConfigureWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Channel.RaiseConfigureWeapon();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            float currentTime = Time.time;
            bool intervalPassed = currentTime - _lastPositionUpdateTime >= UpdateInterval;
            if (!intervalPassed)
            {
                return;
            }

            _lastPositionUpdateTime = currentTime;
            if (context.performed)
            {
                Channel.RaiseMousePosition(context.ReadValue<Vector2>());
            }
        }
    }
}
