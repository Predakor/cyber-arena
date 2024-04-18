using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField] float bulletLive = 5f;


    Rigidbody rb;


    void Start() {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroySelf), bulletLive);
    }


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Health>()) {
            collision.gameObject.GetComponent<Health>().Damage(10);
            DestroySelf();
        }

    }


    void DestroySelf() {
        if (gameObject) {
            Destroy(gameObject);
        }
    }

}
