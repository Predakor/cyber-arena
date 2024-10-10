using System.Collections;
using UnityEngine;

public class GeneralAi : MonoBehaviour {


    [SerializeField] Weapon[] weapons;
    [SerializeField] GameObject target; //target for the AI
    [SerializeField] Collider AgroCollider;

    bool triggered = false;

    [Header("Attack range settings")]
    [SerializeField] float attackRange = 10f;
    [SerializeField] float minRangeToAttack = 1f;
    [SerializeField] float maxRangeToAttack = 10f;

    private void OnTriggerEnter(Collider other) {
        bool canTarget = other.CompareTag("Player");
        if (canTarget && triggered == false) {
            triggered = true;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (triggered && target == other.gameObject) {
            triggered = false;
            target = null;
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


    void Start() {
    }


    // Update is called once per frame
    void Update() {

        if (!triggered || target == null) {
            return;
        }

        //rotate towards player

        bool inAttackRange = attackRange >= minRangeToAttack && maxRangeToAttack <= attackRange;


        if (inAttackRange) {
            Attack(target);
        }
        else {
            if (attackRange > maxRangeToAttack) {
                //move away from player
            }
            if (attackRange < minRangeToAttack) {
                //move towards player
            }
        }

    }
}


