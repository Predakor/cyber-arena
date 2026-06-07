using System;

[Flags]
public enum HitOptions : byte
{
    None = 0,
    Shield = 1 << 0, // Allowed to hit shields
    Armor = 1 << 1, // Allowed to interact with armor
    Health = 1 << 2, // Allowed to hit health
    NoSpillover = 1 << 3, // If a layer absorbs damage, don't let it spill to the next

    // High-level archetypes built out of your targeting rules
    Standard = Shield | Health,
    ShieldOnly = Shield | NoSpillover,
    ArmorOnly = Armor | NoSpillover,
    HealthOnly = Health
}

public interface IDamageable
{
    void Damage(int damage, HitOptions options = HitOptions.None);
}
