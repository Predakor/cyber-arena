using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] float _duration = 5f;
    [SerializeField] float _damage = 5f;

    [SerializeField] GameObject _vfx;

    Rigidbody _rb;

    [Header("Events")]
    public UnityEvent onFire;
    public UnityEvent onDamage;
    public UnityEvent onHit;
    public UnityEvent onDestroy;

    void Awake() {
        _rb = GetComponent<Rigidbody>();

    }

    public void Initialize(Transform spawnPoint, float speed, float damage) {
        _damage = damage;
        gameObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        _rb.velocity = spawnPoint.forward * speed;
        Invoke(nameof(SelfDestroy), _duration);
        onFire?.Invoke();

    }

    void OnTriggerEnter(Collider other) {
        Health objectToDamage = other.gameObject.GetComponent<Health>();

        Debug.Log(other.name, other);
        onHit?.Invoke();


        if (objectToDamage) {
            objectToDamage.Damage((int)_damage);
            onDamage?.Invoke();
        }

        Instantiate(_vfx, transform.position, other.transform.rotation, transform.parent);

        SelfDestroy();
    }

    void SelfDestroy() {
        //implement object pooling
        if (gameObject) {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }

}
