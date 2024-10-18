using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 5f;

    [SerializeField] Transform moveTowards;


    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 1.0f;

    [SerializeField] Transform rotateTowards;

    [SerializeField]
    PlayerInputHandler inputHandler;
    Vector3 currentMovement;

    Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        //inputHandledsr = PlayerInputHandler.Instance;
    }

    public void MoveTowards(Transform _target) {
        moveTowards = _target;
    }
    public void RotateTowards(Transform _target) {
        rotateTowards = _target;
    }

    void FixedUpdate() {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement() {
        Vector3 direction;

        if (inputHandler != null) {
            float moveX = inputHandler.MoveInput.y;
            float moveY = inputHandler.MoveInput.x * -1;

            // Convert input into isometric direction
            float isometricX = (moveX - moveY) / Mathf.Sqrt(2);
            float isometricZ = (moveX + moveY) / Mathf.Sqrt(2);

            direction = new Vector3(isometricX, 0f, isometricZ).normalized;
        }
        else if (moveTowards != null) {
            direction = (moveTowards.position - transform.position).normalized;
            direction.y = 0f;
        }
        else {
            return;
        }

        rb.velocity = direction * walkSpeed;
    }

    void HandleRotation() {
        if (rotateTowards == null) {
            return;
        }

        Vector3 direction = (rotateTowards.position - transform.position).normalized;
        direction.y = 0f;

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
