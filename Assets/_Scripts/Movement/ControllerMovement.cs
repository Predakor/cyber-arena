using Systems.Inputs.Channels;
using UnityEngine;

public sealed class ControllerMovement : BaseMovement
{
    [Header("References")]
    [SerializeField] private InputsChannel inputsChannell;
    [SerializeField] private Dash dashScript;
    [SerializeField] private Vector2 direction;
    private void Awake()
    {
        Debug.Assert(inputsChannell != null, "Player inputs channel is missing");
    }

    private void OnEnable()
    {
        inputsChannell.Subscribe<InputEvents.Move>(x => direction = x.Direction, destroyCancellationToken);
    }
    public void DashTowardsMouse()
    {
        if (dashScript != null)
        {
            dashScript.StartDash(rotateTowards.position);
        }
    }

    protected override void HandleMovement()
    {
        float moveX = direction.y;
        float moveY = direction.x * -1;

        // Convert input into isometric direction
        float isometricX = (moveX - moveY) / Mathf.Sqrt(2);
        float isometricZ = (moveX + moveY) / Mathf.Sqrt(2);

        moveDirection.Set(isometricX, 0f, isometricZ);

        base.HandleMovement();
    }

    protected override void HandleRotation()
    {
        base.HandleRotation();
    }
}
