using System;
using UnityEngine;

[RequireComponent(typeof(GeneralHostileAi))]
[RequireComponent(typeof(BaseMovement))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    //TODO move enemy creation to Factory and remove this mockup config
    // In Factory Create enemy based on EnemyConfig ScriptableObject
    // apply floor modifiers  and pass the config to Health constructor
    private static readonly DurabilityConfig mockupHealthConfig = new()
    {
        currentHealth = 100,
        maxHealth = 100,
        currentShield = 100,
        maxShield = 100,
        armor = 100
    };

    #region Dependencies
    [Header("Dependencies")]
    public GeneralHostileAi AI;
    public BaseMovement Controller;
    public Health Health;
    #endregion

    #region events
    public event Action<Enemy> OnDeath;
    #endregion



    public void Freeze() => SetEnemy(false);
    public void ActivateEnemy() => SetEnemy(true);
    public void SetEnemy(bool active)
    {
        AI.enabled = active;
        Controller.enabled = active;
        Health ??= new Health(mockupHealthConfig);
        Health.OnDeath += DeathHandler;
    }

    private void DeathHandler()
    {
        OnDeath?.Invoke(this);
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

}
