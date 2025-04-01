using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GeneralHostileAi : BaseAI {
    [SerializeField] GameObject target; //target for the AI
    [SerializeField] Collider AgroCollider;

    [Header("AI Flags")]
    [SerializeField] bool triggered = false;
    [SerializeField] bool iddle = false;

    [Header("Weapons")]
    [SerializeField] Weapon[] weapons;

    [Header("Events")]
    public UnityEvent OnAggro;
    public UnityEvent OnAggroLost;
    public UnityEvent OnTrigger;
    public UnityEvent OnTriggerLost;
    public UnityEvent OnTargetAcquired;
    public UnityEvent OnTargetLost;

    #region methods

    public virtual void Trigger() {
        SetTarget(target);
        OnTrigger?.Invoke();
    }

    public virtual void Attack() {

    }

    public virtual void SetTarget(GameObject newTarget) {
        if (newTarget == null) {
            OnTargetLost?.Invoke();
        }
        if (newTarget != target) {
            OnTargetAcquired?.Invoke();
        }
        ChangeTarget(newTarget);
    }


    #endregion

    private void ChangeTarget(GameObject newTarget) {
        if (newTarget == null) {
            triggered = false;
            target = null;
            movement.SetMovementTarget(null);
            movement.RotateTowards(null);
            return;
        }

        triggered = true;
        target = newTarget;
        movement.SetMovementTarget(target.transform);
        movement.RotateTowards(target.transform);
    }

    private void OnTriggerEnter(Collider other) {
        bool canTarget = other.CompareTag("Player");
        if (canTarget && triggered == false) {
            triggered = true;
            ChangeTarget(other.gameObject);
            OnAggro?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (triggered && target == other.gameObject) {
            triggered = false;
            ChangeTarget(null);
            OnAggroLost?.Invoke();
        }
    }

    [ContextMenu("weapons/fireAll")]
    void FireAllWeapons() {
        if (weapons.Count() < 1) {
            return;
        }
        IEnumerator AsyncFire() {
            foreach (Weapon weapon in weapons) {
                if (weapon is Gun gun) {
                    gun.Fire();
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(AsyncFire());
    }

    override protected void Update() {

        if (!triggered || target == null) {
            return;
        }

        FireAllWeapons();
    }
}


