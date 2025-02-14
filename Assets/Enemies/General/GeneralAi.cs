using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GeneralAi : MonoBehaviour {
    [SerializeField] Weapon[] weapons;
    [SerializeField] GameObject target; //target for the AI
    [SerializeField] Collider AgroCollider;
    [SerializeField] TargetMovement movement;

    bool triggered = false;

    [Header("Attack range settings")]
    [SerializeField] float attackRange = 10f;
    [SerializeField] float minRangeToAttack = 1f;
    [SerializeField] float maxRangeToAttack = 10f;


    [Header("Events")]
    [SerializeField] UnityEvent OnAggro;
    [SerializeField] UnityEvent OnAggroLost;



    private void ChangeTarget(GameObject _target) {
        target = _target;
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
            OnAggro.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (triggered && target == other.gameObject) {
            triggered = false;
            ChangeTarget(null);
            OnAggroLost.Invoke();
        }
    }

    private void Attack(GameObject _target) {

        IEnumerator AsyncFire() {

            foreach (Weapon weapon in weapons) {
                weapon.Fire();
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(AsyncFire());
    }

    private float CalculateDistance() {
        if (target == null) { return 0; }
        return Vector3.Distance(transform.position, target.transform.position);
    }

    void Update() {

        if (!triggered || target == null) {
            return;
        }

        bool inAttackRange = attackRange >= minRangeToAttack && maxRangeToAttack <= attackRange;
        float distance = CalculateDistance();


        if (inAttackRange) {
            Attack(target);
            return;
        }



        if (distance > maxRangeToAttack) {
            //move away from player
        }
        else {
            //move towards player
        }


    }
}


