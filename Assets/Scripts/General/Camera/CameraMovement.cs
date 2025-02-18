using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] Transform player;
    [SerializeField] Camera cam;

    [Header("Camera settings")]
    [SerializeField][Range(0, 10)] float treshold = 1f;
    [SerializeField][Range(1, 10)] float cameraSpeed = 1f;
    [SerializeField][Range(1, 25)] float zoomDistance = 12f;
    [SerializeField][Range(0, 5)] float zoomSpeed = 1f;

    Vector3 _targetPosition;
    Vector3 _mousePosition;

    public void SetCameraZoom(float zoom) {
        zoomDistance = zoom;

    }

    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (cam == null) {
            cam = Camera.main;
        }
        cam.orthographicSize = zoomDistance;
    }

    void LateUpdate() {
        _mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.y = 0f;

        _targetPosition = (player.position + _mousePosition) / 2f;

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -treshold + player.position.x, treshold + player.position.x);
        _targetPosition.z = Mathf.Clamp(_targetPosition.z, -treshold + player.position.z, treshold + player.position.z);

        Vector3 newPosition = new(_targetPosition.x, transform.position.y, _targetPosition.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraSpeed * Time.deltaTime);

        if (zoomDistance != cam.orthographicSize) {
            float newSize = zoomSpeed * Time.deltaTime;

            if (cam.orthographicSize > zoomDistance) {
                newSize *= -1;
            }

            if (Mathf.Abs(zoomDistance - cam.orthographicSize) < 0.15f) {
                cam.orthographicSize = zoomDistance;
                return;
            }

            cam.orthographicSize += newSize;
        }
    }
}
