using System;
using System.Collections.Generic;
using Systems.Inputs.Channels;
using UnityEngine.InputSystem;

namespace Systems.Inputs.Handlers
{
    internal sealed class UIInputHandler
         : InputHandlerBase, PlayerControls.IUIActions
    {
        public UIInputHandler(InputsChannel channel) : base(channel) { }
        public override void Init(PlayerControls controls, Dictionary<ControlType, Action> enablers, List<InputActionMap> actionMaps)
        {
            controls.UI.SetCallbacks(this);
            enablers.Add(ControlType.UI, () => controls.UI.Enable());
            actionMaps.Add(controls.UI);
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Channel.RaiseConfigureWeapon();
            }
        }

        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

    }
}
