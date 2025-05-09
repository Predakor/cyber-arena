using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {

    [SerializeField] InputActionAsset playerControls;

    [Header("Action Name References")]
    [SerializeField] string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] string move = "Move";
    [SerializeField] string look = "Look";
    [SerializeField] string shoot = "Shoot";
    [SerializeField] string dash = "Dash";

    InputAction moveAction;
    InputAction lookAction;
    InputAction shootAction;
    InputAction dashAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float ShootInput { get; private set; }
    public float DashInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

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
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        shootAction.performed += context => ShootInput = context.ReadValue<float>();
        shootAction.canceled += context => ShootInput = 0;

        dashAction.performed += context => DashInput = context.ReadValue<float>();
        dashAction.canceled += context => DashInput = 0;
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
