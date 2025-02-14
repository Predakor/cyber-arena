using UnityEngine;

public class ControllerMovement : BaseMovement {

    [Header("References")]
    [SerializeField] PlayerInputHandler inputHandler;
    [SerializeField] Dash dashScript;

    bool IsDashing => dashScript != null && dashScript.isDashing;

    void Awake() {
        if (!inputHandler) {
            FindObjectOfType<PlayerInputHandler>();
        }
    }
    void Start() {
        if (inputHandler != null) {
            Debug.LogError("no input handler", this);
        }
    }

    public void DashTowardsMouse() {
        if (dashScript != null) {
            dashScript.StartDash(rotateTowards.position);
        }
    }

    protected override void HandleMovement() {

        if (inputHandler == null) {
            return;
        }

        float moveX = inputHandler.MoveInput.y;
        float moveY = inputHandler.MoveInput.x * -1;

        // Convert input into isometric direction
        float isometricX = (moveX - moveY) / Mathf.Sqrt(2);
        float isometricZ = (moveX + moveY) / Mathf.Sqrt(2);

        moveDirection.Set(isometricX, 0f, isometricZ);

        base.HandleMovement();
    }

    protected override void HandleRotation() {
        base.HandleRotation();
    }
}
