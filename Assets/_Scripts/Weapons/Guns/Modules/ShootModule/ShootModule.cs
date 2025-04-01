using UnityEngine;

public abstract class ShootModule : MonoBehaviour {
    [SerializeField] protected Projectile _projectile;
    [SerializeField] protected Transform _bulletStart;

    public abstract void Shoot();
    public abstract void Shoot(Projectile projectile);

}
