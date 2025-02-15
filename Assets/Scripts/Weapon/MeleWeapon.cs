using System.Collections;
using UnityEngine;

public class MeleWeapon : Weapon {


    [Header("Weapon stats")]
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float attackDamage = 0.5f;
    [SerializeField] float attackRadius = 0.5f;

    [Header("Weapon events")]


    float _attackCooldown = 0f;
    bool _isRealoading = false;

    void Start() {
        _attackCooldown = Time.time;
    }

    private void OnEnable() {

    }

    public void PickUp() {
        WeaponManager.instance.PickupNewWeapon(gameObject);
        Destroy(gameObject);
    }

    public void Inspect() {
        throw new System.NotImplementedException();
    }

    [ContextMenu("Attack")]
    public void Attack() {

        if (_attackCooldown > Time.time || _isRealoading) {
            return;
        }
        Attack();
        //set flag for the animation


        //wait for the end of the attack

    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(attackDuration);
    }
}
