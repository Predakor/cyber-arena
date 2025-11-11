using Systems.Channels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Inputs {
    public sealed class InputsHandler : MonoBehaviour {
        [SerializeField]
        InputActionAsset playerControls;

        [SerializeField]
        InputsChannel _channel;

        [Header("Action Name References")]
        [SerializeField]
        string actionMapName = "Player";

        [Header("Action Name References")]
        [SerializeField]
        string move = "Move";

        [SerializeField]
        string look = "Look";

        [SerializeField]
        string shoot = "Shoot";

        [SerializeField]
        string dash = "Dash";

        InputAction moveAction;
        InputAction lookAction;
        InputAction shootAction;
        InputAction dashAction;

        public static InputsHandler Instance { get; private set; }
        void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }

            moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
            lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
            shootAction = playerControls.FindActionMap(actionMapName).FindAction(shoot);
            dashAction = playerControls.FindActionMap(actionMapName).FindAction(dash);

            RegisterInputActions();
            gameObject.SetActive(true);
        }

        void RegisterInputActions() {
            moveAction.performed += ct => _channel.RaiseMove(ct.ReadValue<Vector2>());
            moveAction.canceled += _ => _channel.RaiseMove(Vector2.zero);

            lookAction.performed += ct => _channel.RaiseLook(ct.ReadValue<Vector2>());
            lookAction.canceled += _ => _channel.Raise(Vector2.zero);

            shootAction.performed += ct => _channel.RaiseShoot(ct.ReadValue<float>(), true);
            shootAction.canceled += _ => _channel.RaiseShoot(0, true);

            dashAction.performed += _ => _channel.RaiseAbility(true);
            dashAction.canceled += _ => _channel.RaiseAbility(false);
        }

        void OnEnable() {
            moveAction.Enable();
            lookAction.Enable();
            shootAction.Enable();
            dashAction.Enable();
        }

        void OnDisable() {
            moveAction.Disable();
            lookAction.Disable();
            shootAction.Disable();
            dashAction.Disable();
        }
    }
}
