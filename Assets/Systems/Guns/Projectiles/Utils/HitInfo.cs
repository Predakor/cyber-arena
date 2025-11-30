using UnityEngine;

namespace Systems.Guns.Projectiles {
    public readonly struct HitInfo {
        public GameObject Target { get; init; }
        public Vector3 Point { get; init; }
        public Vector3 Normal { get; init; }
    };

}