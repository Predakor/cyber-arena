using System;
using System.Collections.Generic;
using Systems.Inputs.Channels;
using Systems.Inputs.Handlers;
using Systems.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Systems.Inputs
{
    public enum ControlType
    {
        Gameplay = 0,
        UI = 1
    }

    public sealed class InputsManager : Singleton<InputsManager>
    {
        [SerializeField] private InputsChannel _channel;

        private PlayerControls _controls;
        private Dictionary<ControlType, Action> _controlEnablers;
        private List<InputActionMap> _actionMaps;

        protected override void Awake()
        {
            base.Awake();

            _controls = new PlayerControls();
            _controlEnablers = new();
            _actionMaps = new();

            var handlers = new List<InputHandlerBase>
                {
                    new PlayerInputHandler(_channel),
                    new UIInputHandler(_channel),
                };

            foreach (var handler in handlers)
            {
                handler.Init(_controls, _controlEnablers, _actionMaps);
            }


        }

        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => DissableAllMaps();

        private void OnDestroy() => _controls?.Dispose();

        public void EnableControls(ControlType type, bool dissableOthers = false)
        {
            if (_controlEnablers.TryGetValue(type, out var handler))
            {
                if (dissableOthers)
                {
                    DissableAllMaps();
                }
                handler();
            }
        }

        private void DissableAllMaps()
        {
            foreach (var map in _actionMaps)
            {
                map.Disable();
            }
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