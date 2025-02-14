using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetMovement : BaseMovement {

    [SerializeField] Transform moveTowards;


    public void SetMovementTarget(Transform _target) => moveTowards = _target;

    protected override void HandleMovement() {
        if (moveTowards == null) {
            return;
        }

        moveDirection = (moveTowards.position - transform.position).normalized;
        moveDirection.y = 0;

        base.HandleMovement();
    }

    protected override void HandleRotation() {
        base.HandleRotation();
    }
}