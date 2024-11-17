using System.Collections;
using UnityEngine;

public class MeleWeapon : MonoBehaviour {


    [Header("Weapon stats")]
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float attackDamage = 0.5f;
    [SerializeField] float attackRadius = 0.5f;

    [Header("Weapon events")]



    [SerializeField] Transform projectileSpawnPoint;

    AmmoDisplay ammoCountDisplay;
    [SerializeField] bool updateAmmoCounterUI = false;
    [SerializeField] bool isPlayersWeapon = false;
    float _attackCooldown = 0f;
    bool _isRealoading = false;

    void Start() {
        _attackCooldown = Time.time;
    }

    private void OnEnable() {
        if (ammoCountDisplay) {
            ammoCountDisplay.SetAmmoText(0);
        }
    }

    public void PickUp() {
        WeaponManager.instance.PickupNewWeapon(gameObject);
        Destroy(gameObject);
    }

    public void Inspect() {
        throw new System.NotImplementedException();
    }

    [ContextMenu("Fire")]
    public void Fire() {

        if (_attackCooldown > Time.time || _isRealoading) {
            return;
        }
        Attack();
    }

    void Attack() {
        //set flag for the animation


        //wait for the end of the attack


    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(attackDuration);
    }
}
