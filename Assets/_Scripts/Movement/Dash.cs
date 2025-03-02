using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour {

    Rigidbody rb;

    [SerializeField] float dashSpeed = 10.0f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashCooldown = 1.0f;

    [SerializeField] UnityEvent onDashStart;
    [SerializeField] UnityEvent onDashEnd;

    public bool isDashing = false;
    float _dashTime = 0f;
    float _lastDashTime = 0f;

    void Start() {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
    }

    void Update() {
        if (isDashing) {
            _dashTime += Time.deltaTime;
            if (_dashTime >= dashDuration) {
                StopDash();
            }
        }
    }

    [ContextMenu("Start Dash")]
    public void StartDash() {
        if (isDashing || _lastDashTime + dashCooldown > Time.time) {
            return;
        }

        isDashing = true;
        _dashTime = 0f;
        _lastDashTime = Time.time;

        Vector3 dashDirection = transform.forward;
        rb.velocity = dashDirection * dashSpeed;
    }

    public void StartDash(Vector3 direction) {
        if (isDashing || _lastDashTime + dashCooldown > Time.time) {
            return;
        }

        isDashing = true;
        _dashTime = 0f;
        _lastDashTime = Time.time;

        direction -= transform.position;
        rb.velocity = direction.normalized * dashSpeed;

        onDashStart?.Invoke();
    }

    public void StopDash() {
        rb.velocity = Vector3.zero;
        isDashing = false;
        onDashEnd?.Invoke();

    }
}
