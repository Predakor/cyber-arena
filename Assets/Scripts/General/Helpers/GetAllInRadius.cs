using System.Linq;
using UnityEngine;

public class GetAllInRadius : MonoBehaviour {
    [SerializeField] LayerMask _mask;
    [SerializeField] LayerMask _enemyMask;

    [SerializeField] float radius = 5f;

    Collider[] GetAll(Vector3 point, float radius) {
        return Physics.OverlapSphere(point, radius);
    }
    Collider[] GetAll(Vector3 point, float radius, LayerMask mask) {
        return Physics.OverlapSphere(point, radius, mask);
    }

    void Awake() {
        _enemyMask = LayerMask.GetMask("Enemy");
    }

    public Collider[] AllEnemies(Vector3 point, float radius, LayerMask? mask) {
        return GetAll(point, radius, mask ?? _enemyMask);
    }
    public Collider[] AllColliders(Vector3 point, float radius) {
        return GetAll(point, radius);
    }

    public Health[] AllDamageable(Vector3 point, float radius) {
        return GetAll(point, radius)
            .Where(collider => collider.gameObject.GetComponent<Health>() != null)
            .Select(collider => collider.gameObject.GetComponent<Health>())
            .ToArray();
    }
}
