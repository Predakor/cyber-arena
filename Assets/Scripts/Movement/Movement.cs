using UnityEngine;

public class Movement : MonoBehaviour {
    [Header("Mevement Speeds")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 5f;

    [SerializeField]
    PlayerInputHandler inputHandler;
    Vector3 currentMovement;

    Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        inputHandler = PlayerInputHandler.Instance;
    }

    void FixedUpdate() {
        HandleMovement();
        Debug.Log(inputHandler.MoveInput);
    }

    void HandleMovement() {
        float speed = walkSpeed;

        float moveX = inputHandler.MoveInput.x * -1;
        float moveY = inputHandler.MoveInput.y * -1;

        float isometricX = (moveX - moveY) / Mathf.Sqrt(2);
        float isometricZ = (moveX + moveY) / Mathf.Sqrt(2);

        Vector3 isometricDirection = new(isometricX, 0f, isometricZ)
;

        rb.velocity = isometricDirection.normalized * speed;
    }

}
