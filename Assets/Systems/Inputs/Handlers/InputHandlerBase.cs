using System;
using System.Collections.Generic;
using Systems.Inputs.Channels;
using UnityEngine.InputSystem;

namespace Systems.Inputs.Handlers
{
    internal abstract class InputHandlerBase
    {
        protected readonly InputsChannel Channel;

        protected InputHandlerBase() { }
        public InputHandlerBase(InputsChannel channel)
        {
            Channel = channel;
        }

        public abstract void Init(PlayerControls controls, Dictionary<ControlType, Action> enablers, List<InputActionMap> actionMaps);
    }
}