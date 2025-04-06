using System;
using UnityEngine;

public abstract class ProjectileModule : MonoBehaviour {
    public event Action<GameObject> OnImpact;

    [SerializeField] protected int _poolSize = 100;
    [SerializeField] protected int _poolMaxSize = 200;

    public abstract void Init(GunData data);
    public abstract Projectile Get();
    public abstract void AddImpactEffect();
}
