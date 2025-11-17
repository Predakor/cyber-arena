using Assets._Scripts.Utils;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics {
    [RequireComponent(typeof(TrailRenderer), typeof(Rigidbody), typeof(SphereCollider))]
    internal sealed class TrailOnlyProjectile : ProjectileBase<TrailOnlyProjectile> {
        [SerializeField]
        ProjectileConfigurationSO _config;

        [SerializeField]
        TrailRenderer _trail;

        [SerializeField]
        Rigidbody _rigidbody;

        protected override void Awake() {
            base.Awake();
            gameObject.EnsureComponent(out _trail).EnsureComponent(out _rigidbody);
        }

        public override void Shoot() {
            _rigidbody.velocity = Vector3.forward * _config.Speed;
        }

        public override TrailOnlyProjectile Configure(IConfig<TrailOnlyProjectile> config) {
            var c = config as ProjectileConfigurationSO;
            _trail.startWidth = c.Size;
            _trail.endWidth = c.Size / 2;
            _trail.time = c.Size / 2;

            return this;
        }
    }

    public abstract class ProjectileConfigurationSO : ScriptableObject, IConfig//, IProjectileConfig
    {
        protected const string menuPath = "Weapons/Projectiles";
        public float Size;
        public int Damage;
        public float Speed;
        public IReadOnlyList<IHitEffect> Effects;
    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(TrailProjectileConfiguration))]
    public sealed class TrailProjectileConfiguration : ProjectileConfigurationSO, IConfig<TrailOnlyProjectile> {
        public TrailRenderer Trail;
    }
}
