using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] float _duration = 5f;
    [SerializeField] GameObject _vfx;
    [SerializeField] TrailRenderer _trail;

    float _speed = 1.0f;
    Action<Projectile> _killSelf;
    Rigidbody _rb;

    [Header("Events")]
    public UnityEvent onHit;
    public event Action<IDamageable> OnDamageableHit;

    public void Init(Action<Projectile> killSelf, float speed) {
        _speed = speed;
        _killSelf = killSelf;
    }

    public void Reuse(Vector3 position, Quaternion rotation) {
        transform.SetLocalPositionAndRotation(position, rotation);
        if (_trail) {
            _trail.Clear();
        }
        Fire();
    }

    public void Fire() {
        _rb.velocity = transform.forward * _speed;
        StartCoroutine(DestroyAfter(_duration));
    }

    void OnDisable() {
        onHit.RemoveAllListeners();
    }

    void Awake() {
        if (_rb == null) {
            _rb = GetComponent<Rigidbody>();
        }
        if (_trail == null) {
            TryGetComponent(out TrailRenderer trail);
            _trail = trail;
        }
    }

    IEnumerator DestroyAfter(float duration) {
        yield return new WaitForSeconds(duration);
        if (this) {
            _killSelf(this);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IDamageable damageable)) {
            OnDamageableHit?.Invoke(damageable);
        }

        onHit?.Invoke();
        Instantiate(_vfx, transform.position, other.transform.rotation, transform.parent);

        _killSelf(this);
    }

}
