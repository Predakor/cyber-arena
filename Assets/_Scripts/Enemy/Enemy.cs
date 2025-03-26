using System;
using UnityEngine;

[RequireComponent(typeof(GeneralHostileAi))]
[RequireComponent(typeof(BaseMovement))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {

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
    public void SetEnemy(bool active) {
        AI.enabled = active;
        Controller.enabled = active;
        Health.enabled = active;
    }

    void OnEnable() {
        Health.OnHealthChange += CheckIfDeath;
    }

    void OnDisable() {
        Health.OnHealthChange -= CheckIfDeath;
    }

    void CheckIfDeath(int health) {
        if (health < 1) {
            OnDeath?.Invoke(this);
        }
    }
}
