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

    [SerializeField] PlayerInputHandler inputHandler;
    [SerializeField] Animator animator;
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
        Vector3 _direction;

        if (inputHandler != null) {
            float moveX = inputHandler.MoveInput.y;
            float moveY = inputHandler.MoveInput.x * -1;

            // Convert input into isometric direction
            float isometricX = (moveX - moveY) / Mathf.Sqrt(2);
            float isometricZ = (moveX + moveY) / Mathf.Sqrt(2);

            _direction = new Vector3(isometricX, 0f, isometricZ).normalized;
        }
        else if (moveTowards != null) {
            _direction = (moveTowards.position - transform.position).normalized;
            _direction.y = 0f;
        }
        else {
            return;
        }

        bool moving = _direction != Vector3.zero;

        rb.velocity = _direction * walkSpeed;
        animator.SetBool("Running", moving);

    }

    void HandleRotation() {
        if (rotateTowards == null) {
            return;
        }

        Vector3 _direction = (rotateTowards.position - transform.position).normalized;
        _direction.y = 0f;

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(_direction);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        bool _rotating = _direction != Vector3.zero;
        //if moves set animator flag

    }
}
