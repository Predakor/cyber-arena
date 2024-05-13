using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Vector3 _offset;
    [SerializeField] Transform target;
    [SerializeField] float smoothTime;

    Vector3 _currentVelocity = Vector3.zero;



    void Awake() {

        _offset = transform.position - target.position;

    }


    // Update is called once per frame
    void LateUpdate() {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
