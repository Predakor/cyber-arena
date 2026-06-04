using Systems.Inputs.Channels;
using Systems.Shared;
using UnityEngine;

public sealed class MouseProvider : Singleton<MouseProvider>
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private InputsChannel _channel;

    public Vector3 WorldPosition { get; private set; }
    public Vector2 ScreenPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        if (groundMask == 0)
        {
            groundMask = LayerMask.GetMask("Ground");
        }

        _channel.Subscribe<InputEvents.MousePosition>(OnMousePosition, destroyCancellationToken);
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(ScreenPosition);

        if (Physics.Raycast(ray, out var hitInfo, 500f, groundMask))
        {
            WorldPosition = hitInfo.point;
            return;
        }

        Plane horizontalPlane = new(Vector3.up, Vector3.zero);

        if (horizontalPlane.Raycast(ray, out float enter))
        {
            WorldPosition = ray.GetPoint(enter);
        }

    }

    private void LateUpdate() => transform.position = WorldPosition;

    private void OnMousePosition(InputEvents.MousePosition payload) => ScreenPosition = payload.Position;

}
