using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] float duration = 5f;
    [SerializeField] float damage = 5f;

    [SerializeField] GameObject vfx;

    Rigidbody rb;

    [Header("Events")]
    public UnityEvent onFire;
    public UnityEvent onDamage;
    public UnityEvent onHit;
    public UnityEvent onDestroy;

    void Awake() {
        rb = GetComponent<Rigidbody>();

    }

    public void Initialize(Transform spawnPoint, float speed, float _damage) {
        damage = _damage;
        gameObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        rb.velocity = spawnPoint.forward * speed;
        Invoke(nameof(SelfDestroy), duration);
        onFire?.Invoke();

    }

    void OnCollisionEnter(Collision collision) {
        Health objectToDamage = collision.gameObject.GetComponent<Health>();

        if (objectToDamage) {
            objectToDamage.Damage((int)damage);
            onDamage?.Invoke();
        }
        else {
            onHit?.Invoke();
        }

        Instantiate(vfx, transform.position, transform.rotation, transform.parent);

        SelfDestroy();
    }



    void SelfDestroy() {
        //implement object pooling
        if (gameObject) {
            onDestroy?.Invoke();
            Destroy(gameObject, 5f);
        }
    }

}
