using System;
using UnityEngine;

[Serializable]
public sealed class DurabilityConfig
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int currentShield = 0;
    public int maxShield = 0;
    public int armor = 0;
}

public sealed class Health : IDamageable, IHealthMonitor
{
    public int MaxHealth { get; private set; }
    public int MaxShield { get; private set; }
    public int Armor { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentShield { get; private set; }


    public event Action<int> OnHealthChange;
    public event Action<int> OnShieldChange;
    public event Action<int> OnMaxHealthChange;
    public event Action<int> OnMaxShieldChange;
    public event Action<int> OnArmorChange;

    public event Action OnDeath;

    public Health(DurabilityConfig config)
    {
        MaxHealth = config.maxHealth;
        MaxShield = config.maxShield;
        CurrentHealth = config.currentHealth;
        CurrentShield = config.currentShield;
        Armor = config.armor;
    }

    public void Damage(int damage, HitOptions options = HitOptions.None)
    {
        if (CurrentHealth <= 0 || damage <= 0)
        {
            return;
        }

        bool propagateOverflowDamage = !options.HasFlag(HitOptions.NoSpillover);

        if (options.HasFlag(HitOptions.Shield))
        {
            int shieldDamage = Mathf.Min(damage, CurrentShield);
            SetShield(CurrentShield - shieldDamage);
            damage = GetRemainingDamage(damage, shieldDamage, propagateOverflowDamage);
        }

        if (options.HasFlag(HitOptions.Armor))
        {
            int armorDamage = Math.Min(damage, Armor);
            SetArmor(Armor - armorDamage);
            damage = GetRemainingDamage(damage, armorDamage, propagateOverflowDamage);
        }

        if (damage > 0)
        {
            int effectiveArmor = options.HasFlag(HitOptions.HealthOnly) ? 0 : Armor;
            int healthDamage = Mathf.Max(damage - effectiveArmor, 0);
            SetHealth(CurrentHealth - healthDamage);
        }

    }

    private void SetHealth(int value)
    {
        int clamped = Mathf.Clamp(value, 0, MaxHealth);
        if (CurrentHealth == clamped)
        {
            return;
        }

        CurrentHealth = clamped;
        OnHealthChange?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    private void SetShield(int value)
    {
        int clamped = Mathf.Clamp(value, 0, MaxShield);
        if (CurrentShield == clamped)
        {
            return;
        }

        CurrentShield = clamped;
        OnShieldChange?.Invoke(CurrentShield);
    }

    private void SetArmor(int value)
    {
        int clamped = Mathf.Max(0, value);
        if (Armor == clamped)
        {
            return;
        }

        Armor = clamped;
        OnArmorChange?.Invoke(Armor);

    }

    private int GetRemainingDamage(int originalDamage, int dealtDamage, bool hasPropagateDamage)
    {
        return hasPropagateDamage
            ? originalDamage - dealtDamage
            : 0;

    }

}