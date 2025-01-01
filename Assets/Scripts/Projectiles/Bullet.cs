using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField] float bulletLive = 5f;
    [SerializeField] Mask collisionMask;

    [SerializeField]
    GameObject vfx;

    Rigidbody rb;

    [Header("Events")]
    [SerializeField] UnityEvent onFire;
    [SerializeField] UnityEvent onDamage;
    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent onDestroy;

    void Start() {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroySelf), bulletLive);
    }


    void OnCollisionEnter(Collision collision) {
        Health collisionHealth = collision.gameObject.GetComponent<Health>();

        if (collisionHealth) {
            collisionHealth.Damage(10);
            Instantiate(vfx, transform.position, transform.rotation, transform.parent);
            onDamage?.Invoke();
        }
        else {
            onHit?.Invoke();
        }
        DestroySelf();
    }
    void DestroySelf() {
        //implement object pooling
        if (gameObject) {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }

}
