using UnityEngine;

public sealed class SimpleHitScanWeapon : SimpleWeapon
{
    [SerializeField] private LayerMask _targetLayer;

    protected override void Awake()
    {
        base.Awake();

        if (_targetLayer.value == 0)
        {
            _targetLayer = LayerMask.GetMask("Default");
        }
    }

    protected override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out var hit, _range, _targetLayer))
        {
            Debug.DrawLine(origin, hit.point, Color.red, 0.15f);

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(_damage);
            }
        }
    }

}
