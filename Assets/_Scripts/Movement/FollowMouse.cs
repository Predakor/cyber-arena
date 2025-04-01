using UnityEngine;

public class FollowMouse : MonoBehaviour {

    [SerializeField] Camera _camera;
    [SerializeField] LayerMask groundMask;

    Vector3 mousePosition = Vector3.zero;

    public Vector3 MousePosition { get => mousePosition; }

    void Awake() {
        if (_camera == null) {
            _camera = Camera.main;
        }
        if (groundMask == 0) {
            groundMask = LayerMask.GetMask("Ground");
        }
    }

    void Update() {
        var (succes, position) = GetMousePosition();
        if (succes) {
            transform.position = position;
            mousePosition = position;
        }
    }

    (bool succes, Vector3 position) GetMousePosition() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask)) {
            return (succes: true, position: hitInfo.point);
        }
        return (succes: false, position: Vector3.zero);
    }
}
