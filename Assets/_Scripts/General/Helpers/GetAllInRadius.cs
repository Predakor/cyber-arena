using System.Collections.Generic;
using UnityEngine;

public class GetAllInRadius : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private LayerMask _enemyMask;

    [SerializeField] private float radius = 5f;

    private Collider[] GetAll(Vector3 point, float radius) => Physics.OverlapSphere(point, radius);

    private Collider[] GetAll(Vector3 point, float radius, LayerMask mask) => Physics.OverlapSphere(point, radius, mask);

    private void Awake()
    {
        _enemyMask = LayerMask.GetMask("Enemy");
    }

    public Collider[] AllEnemies(Vector3 point, float radius, LayerMask? mask)
    {
        return GetAll(point, radius, mask ?? _enemyMask);
    }
    public Collider[] AllColliders(Vector3 point, float radius)
    {
        return GetAll(point, radius);
    }

    public List<IDamageable> AllDamageable(Vector3 point, float radius)
    {
        Collider[] colliders = GetAll(point, radius);
        //with correct matrix settings we should only collide with damageable things or walls
        List<IDamageable> targets = new(colliders.Length);

        foreach (var colider in colliders)
        {
            if (colider.TryGetComponent<IDamageable>(out var damageable))
            {
                targets.Add(damageable);
            }
        }

        return targets;
    }
}
