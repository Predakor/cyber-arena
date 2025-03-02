using UnityEngine;

public class BaseMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] protected float walkSpeed = 3f;
    [SerializeField] protected float sprintSpeed = 5f;
    [SerializeField] protected Vector3 moveDirection;

    [Header("Rotation")]
    [SerializeField][Range(0, 360)] protected float rotationSpeed = 90f;
    [SerializeField][Range(0, 180)] protected float rotationTreshold = 0f;
    [SerializeField] protected Vector3 rotationDirection;
    [SerializeField] protected Transform rotateTowards;

    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rb;


    [Header("Movement flags")]
    [SerializeField] bool canMove = true;
    [SerializeField] bool canRotate = true;

    public bool CanMove { get => canMove; }
    public bool CanRotate { get => canRotate; }

    public void RotateTowards(Transform _target) => rotateTowards = _target;
    public void AllowRotation(bool allow = true) => canRotate = allow;
    public void AllowMovement(bool allow = true) => canMove = allow;

    void Awake() {
        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }
        if (animator == null) {
            animator = GetComponent<Animator>();
        }
    }

    void Start() {
        if (!rb) { Debug.LogError("no rigidbody", this); }
        if (!animator) Debug.LogError("no animator", this);
    }

    void FixedUpdate() {
        if (CanRotate) {
            HandleRotation();
        }
        if (CanMove) {
            HandleMovement();
        }
    }


    protected virtual void HandleMovement() {
        rb.velocity = walkSpeed * moveDirection;
        bool _moving = moveDirection != Vector3.zero;

        if (animator && animator.GetBool("Moving") != _moving) {
            animator.SetBool("Moving", _moving);
        }
    }

    protected virtual void HandleRotation() {
        if (rotateTowards == null) {
            return;
        }

        //check if object is to close
        if ((transform.position - rotateTowards.position).sqrMagnitude < Mathf.Epsilon) {
            return;
        }

        Vector3 objectPosition = new(transform.position.x, 0, transform.position.z);
        Vector3 targetPosition = new(rotateTowards.position.x, 0, rotateTowards.position.z);

        rotationDirection = (targetPosition - objectPosition).normalized;

        float _angle = Vector3.Angle(transform.forward, rotationDirection);

        if (_angle > rotationTreshold) {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        bool _rotating = rotationDirection != Vector3.zero;

        if (animator && animator.GetBool("Rotating") != _rotating) {
            animator.SetBool("Rotating", _rotating);
        }
    }
}
