using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
    #region Stats
    [Header("Health stats")]
    [SerializeField] int currentHealth = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentShield = 0;
    [SerializeField] int maxShield = 0;
    [SerializeField] int armor = 0;

    [Header("Invincibility frames")]
    [SerializeField] bool InvincibilityFrames = true;
    [SerializeField] float InvicibilityFramesCooldown = 0.5f;
    [SerializeField] float InvicibilityFramesDuration = 0.1f;
    #endregion

    public event Action<int> OnHealthChange;
    public event Action<int> OnShieldChange;
    public event Action OnIFramesStart;
    public event Action OnIFramesEnd;

    public bool InvincibilityFramesActive { get; private set; } = false;
    private bool _iFramesReady = true;
    private bool _canBeDamaged = true;

    public int MaxHealth { get => maxHealth; private set => maxHealth = value; }
    public int MaxShield { get => maxShield; private set => maxShield = value; }
    public int Armor { get => armor; private set => armor = value; }

    public int CurrentHealth {
        get => currentHealth;
        private set {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChange?.Invoke(currentHealth);
        }
    }

    public int CurrentShield {
        get => currentShield;
        private set {
            currentShield = Mathf.Clamp(value, 0, maxShield);
            OnShieldChange?.Invoke(currentShield);
        }
    }

    public void Damage(int damage, bool ignoreShields = false, bool ignoreArmor = false) {
        if (!_canBeDamaged) return;

        if (CurrentShield > 0 && !ignoreShields) {
            int shieldDamage = Mathf.Min(damage, CurrentShield);
            CurrentShield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0) {
            CurrentHealth -= Mathf.Max(damage - armor, 0);
        }

        if (CurrentHealth <= 0) {
            Destroy(gameObject);
        }
        else if (InvincibilityFrames && _iFramesReady) {
            StartCoroutine(StartInvincibilityFrames());
        }
    }

    private IEnumerator StartInvincibilityFrames() {
        _canBeDamaged = false;
        InvincibilityFramesActive = true;

        OnIFramesStart?.Invoke();

        yield return new WaitForSeconds(InvicibilityFramesDuration);

        _canBeDamaged = true;
        InvincibilityFramesActive = false;
        _iFramesReady = false;

        OnIFramesEnd?.Invoke();

        yield return new WaitForSeconds(InvicibilityFramesCooldown);
        _iFramesReady = true;
    }

    public void DamageHealth(int damage) {
        Damage(damage, true);
    }

    public void DamageShield(int damage) {
        CurrentShield -= damage;
    }

    public void DamageArmor(int damage) {
        armor = Mathf.Max(armor - damage, 0);
    }
}
