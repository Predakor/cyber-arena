public interface IDamageable {
    void Damage(int damage, bool ignoreShields = false, bool ignoreArmor = false);
}
