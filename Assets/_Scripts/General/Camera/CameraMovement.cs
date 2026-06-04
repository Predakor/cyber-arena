using System;
using Systems.Inputs.Channels;
using UnityEngine;

public sealed class CameraMovement : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    [SerializeField] private InputsChannel _channel;

    [Header("Camera Tracking Feel")]
    [SerializeField][Range(0f, 5f)] private float centerDeadzone = 2.5f; // Mouse ignores camera within this radius
    [SerializeField][Range(0.1f, 1f)] private float mousePullWeight = 0.3f; // Lower = less aggressive tracking (Replaces / 2f)

    [Header("Camera Bounds")]
    [SerializeField][Range(0, 15)] private float tresholdX = 10f;
    [SerializeField][Range(0, 15)] private float tresholdY = 10f;
    [SerializeField][Range(1, 3)] private float downwardModifier = 1.5f;

    [Header("Camera Speed")]
    [SerializeField][Range(1, 10)] private float cameraSpeed = 4f; // Might need to bump this up for smoother Lerping
    [SerializeField][Range(1, 25)] private float zoomDistance = 12f;
    [SerializeField][Range(0, 5)] private float zoomSpeed = 1f;

    private Vector3 _targetPosition;
    private MouseProvider _mouseProvider;
    public void SetCameraZoom(float zoom)
    {
        zoomDistance = zoom;
    }

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameManager.Instance.Player.transform;
        }

        cam.orthographicSize = zoomDistance;
        _mouseProvider = MouseProvider.Instance;
    }

    private void LateUpdate()
    {
        HandleTransition();
        HandleZoom();
    }

    private void HandleTransition()
    {
        Vector3 mousePos = _mouseProvider.WorldPosition;

        // 1. Get Isometric Screen Axes
        Vector3 screenRight = cam.transform.right;
        screenRight.y = 0f;
        screenRight.Normalize();
        Vector3 screenForward = Vector3.Cross(screenRight, Vector3.up).normalized;

        // 2. Get Raw Distance
        Vector3 rawWorldOffset = mousePos - player.position;
        float distance = rawWorldOffset.magnitude;

        // 3. Apply SMOOTH DEADZONE
        // Subtracts the deadzone from the actual distance. If negative, floors it to 0.
        // This completely eliminates center jitter without causing a sudden "snap" when you leave the deadzone.
        float activeDistance = Mathf.Max(0, distance - centerDeadzone);
        Vector3 activeOffset = rawWorldOffset.normalized * activeDistance;

        // 4. Convert to Screen Space applying the customizable Pull Weight
        float screenOffsetX = Vector3.Dot(activeOffset, screenRight) * mousePullWeight;
        float screenOffsetZ = Vector3.Dot(activeOffset, screenForward) * mousePullWeight;

        // 5. Downward Modifier
        float finalBottomThreshold = tresholdY;
        if (screenOffsetZ < 0)
        {
            screenOffsetZ *= downwardModifier;
            finalBottomThreshold *= downwardModifier;
        }

        // 6. Clamp
        float clampedX = Mathf.Clamp(screenOffsetX, -tresholdX, tresholdX);
        float clampedZ = Mathf.Clamp(screenOffsetZ, -finalBottomThreshold, tresholdY);

        // 7. Apply back to world coordinates
        Vector3 finalWorldOffset = (screenRight * clampedX) + (screenForward * clampedZ);

        _targetPosition = player.position + finalWorldOffset;
        _targetPosition.y = transform.position.y;

        // 8. Smooth transition
        transform.position = Vector3.Lerp(transform.position, _targetPosition, cameraSpeed * Time.deltaTime);
    }

    private void HandleZoom()
    {
        if (zoomDistance == cam.orthographicSize)
        {
            return;
        }

        float newSize = zoomSpeed * Time.deltaTime;

        if (cam.orthographicSize > zoomDistance)
        {
            newSize *= -1;
        }

        if (Mathf.Abs(zoomDistance - cam.orthographicSize) < 0.15f)
        {
            cam.orthographicSize = zoomDistance;
            return;
        }

        cam.orthographicSize += newSize;
    }
}
