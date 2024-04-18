using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {
    [Header("Movement Speeds")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 5f;

    [SerializeField]
    PlayerInputHandler inputHandler;
    Vector3 currentMovement;


    [Header("Player rotation")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Camera camera;

    Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    void Start() {
        inputHandler = PlayerInputHandler.Instance;
    }

    void FixedUpdate() {
        HandleMovement();
        HandleRotation();
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

    void HandleRotation() {

        var (succes, position) = GetMousePosition();
        if (!succes) { return; }

        Vector3 direction = position - transform.position;
        direction.y = 0f;
        transform.forward = direction;
    }

    (bool succes, Vector3 position) GetMousePosition() {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask)) {
            return (succes: true, position: hitInfo.point);
        }
        return (succes: false, position: Vector3.zero);
    }
}
