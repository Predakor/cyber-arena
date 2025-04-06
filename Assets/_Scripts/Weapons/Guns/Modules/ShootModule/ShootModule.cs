using UnityEngine;

public abstract class ShootModule : MonoBehaviour {
    [SerializeField] protected Projectile _projectile;
    public abstract void Shoot(Projectile projectile);
}
