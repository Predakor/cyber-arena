using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {

    [Header("Health stats")]
    [SerializeField] int currentHealth = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int shield = 0;
    [SerializeField] int maxShield = 0;
    [SerializeField] int armor = 0;

    [Header("Invincibility frames")]
    [SerializeField] bool InvincibilityFrames = true;
    [SerializeField] float InvicibilityFramesCooldown = 0.5f;
    [SerializeField] float InvicibilityFramesDuration = 0.1f;
    public bool InvincibilityFramesActive { get; private set; } = false;
    private bool invincibilityFramesReady = true;

    public int CurrentHealth { get => currentHealth; }
    public int MaxHealth { get => maxHealth; }
    public int Shield { get => shield; }
    public int MaxShield { get => shield; }
    public int Armor { get => armor; }

    bool canBeDamaged = true;

    public void Damage(int damage, bool ignoreShields = false, bool ignoreArmor = false) {
        if (!canBeDamaged) {
            return;
        }

        if (shield > 0 && !ignoreShields) {
            int remainingDamage = shield - damage;
            shield -= damage;
            if (remainingDamage > 0) {
                currentHealth -= remainingDamage - armor;
            }
        }
        else {
            currentHealth -= damage - armor;
        }

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
        else if (InvincibilityFrames && invincibilityFramesReady) {
            StartCoroutine(StartInvincibilityFrames());
        }
    }


    private IEnumerator StartInvincibilityFrames() {
        canBeDamaged = false;
        InvincibilityFramesActive = true;

        yield return new WaitForSeconds(InvicibilityFramesDuration);
        canBeDamaged = true;
        InvincibilityFramesActive = false;
        invincibilityFramesReady = false;

        yield return new WaitForSeconds(InvicibilityFramesCooldown);
        invincibilityFramesReady = true;
    }

    public void DamageHealth(int damage) {
        Damage(damage, true);
    }
    public void DamageShield(int damage) {
        shield -= damage;
    }
    public void DamageArmor(int damage) {
        armor -= damage;
    }
}
