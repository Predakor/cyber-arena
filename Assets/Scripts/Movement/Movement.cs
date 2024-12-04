using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 5f;

    [SerializeField] Transform moveTowards;

    [Header("Rotation")]
    [SerializeField][Range(0, 360)] float rotationSpeed = 1.0f;
    [SerializeField][Range(0, 180)] float rotationTreshold = 1.0f;
    [SerializeField] Transform rotateTowards;

    [Header("References")]
    [SerializeField] PlayerInputHandler inputHandler;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;

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

        if (animator) {
            animator.SetBool("Running", moving);
        }
    }

    void HandleRotation() {

        if (rotateTowards == null) {
            return;
        }

        //check if object is to close
        if ((transform.position - rotateTowards.position).sqrMagnitude < Mathf.Epsilon) {
            return;
        }

        Vector3 objectPosition = new(transform.position.x, 0, transform.position.z);
        Vector3 targetPosition = new(rotateTowards.position.x, 0, rotateTowards.position.z);

        Vector3 _direction = (targetPosition - objectPosition).normalized;

        float _angle = Vector3.Angle(transform.forward, _direction);

        Debug.Log(_angle);
        Debug.DrawLine(objectPosition, targetPosition);

        if (_angle > rotationTreshold) {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(_direction);

            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}