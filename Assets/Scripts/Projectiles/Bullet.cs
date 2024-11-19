using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField] float bulletLive = 5f;
    [SerializeField] Mask collisionMask;

    Rigidbody rb;


    void Start() {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroySelf), bulletLive);
    }


    void OnCollisionEnter(Collision collision) {
        Health collisionHealth = collision.gameObject.GetComponent<Health>();

        if (collisionHealth) {
            collisionHealth.Damage(10);
        }
        DestroySelf();
    }
    void DestroySelf() {
        if (gameObject) {
            Destroy(gameObject);
        }
    }

}
