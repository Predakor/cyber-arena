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
        public PlayerInputHandler(InputsChannel channel) : base(channel) { }

        public override void Init(PlayerControls controls, Dictionary<ControlType, Action> enablers, List<InputActionMap> actionMaps)
        {
            controls.Player.SetCallbacks(this);
            enablers.Add(ControlType.UI, () => controls.UI.Enable());
            actionMaps.Add(controls.UI);
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
            if (context.performed)
            {
                Channel.RaiseLook(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                Channel.RaiseLook(Vector2.zero);
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

    }
}
