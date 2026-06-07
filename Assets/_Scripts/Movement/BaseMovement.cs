using Assets.Scripts.Utils;
using System.Linq;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    private static readonly int RotatingHash = Animator.StringToHash("Rotating");
    private static readonly int MovingHash = Animator.StringToHash("Moving");

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
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canRotate = true;


    private bool _hasRotatingAnimation;
    private bool _hasMovingAnimation;

    public bool CanMove { get => canMove; }
    public bool CanRotate { get => canRotate; }

    public void RotateTowards(Transform _target) => rotateTowards = _target;
    public void AllowRotation(bool allow = true) => canRotate = allow;
    public void AllowMovement(bool allow = true) => canMove = allow;

    private void Awake()
    {
        gameObject
            .EnsureComponent(out rb)
            .EnsureComponent(out animator);
    }

    private void Start()
    {
        _hasRotatingAnimation = animator && animator.parameters.Any(x => x.nameHash == RotatingHash);
        _hasMovingAnimation = animator && animator.parameters.Any(x => x.nameHash == MovingHash);
        Debug.Assert(_hasRotatingAnimation || _hasMovingAnimation, "Animator does not have required parameters for movement or rotation animations");
    }

    private void FixedUpdate()
    {
        if (CanRotate)
        {
            HandleRotation();
        }
        if (CanMove)
        {
            HandleMovement();
        }
    }


    protected virtual void HandleMovement()
    {
        rb.linearVelocity = walkSpeed * moveDirection;
        bool _moving = moveDirection != Vector3.zero;

        if (_hasMovingAnimation && animator.GetBool(MovingHash) != _moving)
        {
            animator.SetBool(MovingHash, _moving);
        }
    }

    protected virtual void HandleRotation()
    {
        if (rotateTowards == null)
        {
            return;
        }

        //check if object is to close
        if ((transform.position - rotateTowards.position).sqrMagnitude < Mathf.Epsilon)
        {
            return;
        }

        Vector3 objectPosition = new(transform.position.x, 0, transform.position.z);
        Vector3 targetPosition = new(rotateTowards.position.x, 0, rotateTowards.position.z);

        rotationDirection = (targetPosition - objectPosition).normalized;

        float _angle = Vector3.Angle(transform.forward, rotationDirection);

        if (_angle > rotationTreshold)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        bool _rotating = rotationDirection != Vector3.zero;

        if (_hasRotatingAnimation && animator.GetBool(RotatingHash) != _rotating)
        {
            animator.SetBool(RotatingHash, _rotating);
        }
    }
}
