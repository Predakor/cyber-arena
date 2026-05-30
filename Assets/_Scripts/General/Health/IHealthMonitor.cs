using System;

public interface IHealthMonitor
{
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int MaxShield { get; }
    int CurrentShield { get; }

    event Action<int> OnHealthChange;
    event Action<int> OnShieldChange;
    event Action<int> OnMaxHealthChange;
    event Action<int> OnMaxShieldChange;
}
