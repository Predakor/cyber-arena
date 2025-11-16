using Assets._Scripts.Utils;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics {
    [RequireComponent(typeof(TrailRenderer), typeof(Rigidbody), typeof(SphereCollider))]
    internal sealed class TrailOnlyProjectile : MonoBehaviour, IProjectile {
        [SerializeField]
        ProjectileConfigurationSO _config;

        [SerializeField]
        TrailRenderer _trail;

        [SerializeField]
        Rigidbody _rigidbody;

        [SerializeField]
        Collider _collider;

        private void Awake() {
            gameObject
                .EnsureComponent(out _trail)
                .EnsureComponent(out _rigidbody)
                .EnsureComponent(out _collider);
        }

        public void Configure(IProjectileConfig configuration) {
            _config = configuration as ProjectileConfigurationSO;
            _trail.startWidth = _config.Size;
            _trail.endWidth = _config.Size;
            _trail.time = _config.Size / 2;
        }

        public void Shoot() {
            _rigidbody.velocity = Vector3.forward * _config.Speed;
        }
    }

    public abstract class ProjectileConfigurationSO : ScriptableObject //, IProjectileConfig
    {
        protected const string menuPath = "Weapons/Projectiles";
        public float Size;
        public int Damage;
        public float Speed;
        public IReadOnlyList<IHitEffect> Effects;
    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(TrailProjectileConfiguration))]
    public sealed class TrailProjectileConfiguration : ProjectileConfigurationSO {
        public TrailRenderer Trail;
    }
}
