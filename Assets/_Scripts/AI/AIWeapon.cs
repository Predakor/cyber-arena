using UnityEngine;

public sealed class AIWeapon : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _range = 20f;
    [SerializeField][Range(0.1f, 20f)] private float _fireRate = 1f;

    private float _nextFireTime;

    public bool TryFire(Transform target)
    {
        if (Time.time < _nextFireTime)
        {
            return false;
        }

        _nextFireTime = Time.time + (1f / _fireRate);

        var origin = _muzzle != null ? _muzzle.position : transform.position;
        var direction = (target.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out var hit, _range))
        {
            if (hit.collider.TryGetComponent<Health>(out var health))
            {
                health.Damage(_damage);
            }
        }

        return true;
    }
}