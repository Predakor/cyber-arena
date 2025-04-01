using System;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] float _duration = 5f;
    [SerializeField] GameObject _vfx;

    float _speed = 1.0f;
    Action<Projectile> _killSelf;
    Rigidbody _rb;

    [Header("Events")]
    public UnityEvent onHit;
    public event Action<IDamageable> OnDamageableHit;

    void Awake() {
        if (_rb == null) {
            _rb = GetComponent<Rigidbody>();
        }
    }

    public void Init(Action<Projectile> killSelf, Vector3 position, float speed) {
        transform.SetLocalPositionAndRotation(position, Quaternion.identity);
        _speed = speed;
        _killSelf = killSelf;
    }

    public void Fire() {
        _rb.velocity = transform.forward * _speed;
        Invoke(nameof(_killSelf), _duration);

    }

    void OnDisable() {
        onHit.RemoveAllListeners();
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IDamageable damageable)) {
            OnDamageableHit(damageable);
        }

        onHit.Invoke();
        Instantiate(_vfx, transform.position, other.transform.rotation, transform.parent);

        _killSelf(this);
    }

}
