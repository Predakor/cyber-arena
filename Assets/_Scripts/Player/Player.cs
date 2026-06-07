using UnityEngine;

namespace Scripts.Player
{
    /// <summary>
    /// Empty markup player for player identification for now
    /// </summary>
    public sealed class Player : MonoBehaviour, IDamageable
    {
        [SerializeReference] private Health _health = new();
        public IHealthMonitor HealthMonitor => _health;

        public void Damage()
        {
            _health.Damage(10);
        }

        public void Damage(int damage, HitOptions options = HitOptions.None) => _health.Damage(damage, options);

        private void Awake()
        {
            _health.Init(new()
            {
                maxHealth = 100,
                currentHealth = 100,
                armor = 1,
                currentShield = 50,
                maxShield = 50
            });

        }
    }
}
