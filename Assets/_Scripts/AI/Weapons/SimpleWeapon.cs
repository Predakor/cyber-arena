using UnityEngine;

public abstract class SimpleWeapon : MonoBehaviour
{
    [SerializeField] protected Transform _muzzle;

    [Range(1, 50)]
    [SerializeField] protected int _damage = 10;

    [Range(1, 50)]
    [SerializeField] protected float _range = 20f;

    [Range(0.1f, 20f)]
    [SerializeField] protected float _fireRate = 1f;


    protected float _nextFireTime;

    protected virtual void Awake()
    {
        if (_muzzle == null)
        {
            _muzzle = transform;
        }
        _nextFireTime = Time.time;
    }

    public bool TryFire()
    {
        if (!IsReadyToShoot)
        {
            return false;
        }

        _nextFireTime = Time.time + (1f / _fireRate);
        Shoot(_muzzle.position, _muzzle.forward);

        return true;
    }

    protected abstract void Shoot(Vector3 position, Vector3 direction);

    protected bool IsReadyToShoot => Time.time > _nextFireTime;

}
