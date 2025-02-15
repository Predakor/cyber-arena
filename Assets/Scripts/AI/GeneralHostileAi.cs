using System.Collections;
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
    [SerializeField] UnityEvent OnAggro;
    [SerializeField] UnityEvent OnAggroLost;

    #region methods
    public virtual void Attack() {

    }
    public virtual void SetTarget(GameObject _target) {
        target = _target;
    }
    public virtual void OnTrigger() {

    }
    public virtual void OnTriggerLost() {

    }
    public virtual void OnTargetAcquired() {

    }
    public virtual void OnTargetLost() {

    }
    #endregion

    private void ChangeTarget(GameObject _target) {
        if (target != null) {
            movement.SetMovementTarget(target.transform);
            movement.RotateTowards(target.transform);
        }
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

        IEnumerator AsyncFire() {

            foreach (Weapon weapon in weapons) {
                weapon.Fire();
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


