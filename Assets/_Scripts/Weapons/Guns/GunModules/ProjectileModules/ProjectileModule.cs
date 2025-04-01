using System;
using UnityEngine;


public interface IProjectileModule {
    public void Generate();
    public void Init();
    public void AddImpactEffect();
}
public abstract class ProjectileModule : MonoBehaviour, IProjectileModule {
    public event Action<GameObject> OnImpact;

    [SerializeField] protected int _poolSize = 100;
    [SerializeField] protected int _poolMaxSize = 200;

    public abstract void AddImpactEffect();
    public abstract void Generate();
    public abstract void Init();
}
