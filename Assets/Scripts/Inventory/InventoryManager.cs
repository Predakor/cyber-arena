using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    [SerializeField] Collider PickuUpCollider;

    public List<Weapon> weapons;



    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IPickable item)) {
            item.PickUp();
        }
    }


    public void AddWeapon(Weapon weapon) {
        if (weapons.Contains(weapon)) {
            return;
        }
        weapons.Add(weapon);
    }
    public void RemoveWeapon(Weapon weapon) {
        if (!weapons.Contains(weapon)) {
            return;
        }
        weapons.Remove(weapon);
    }

    public bool ContainsWeapon(Weapon weapon) {
        return weapons.Contains(weapon);
    }


    void Start() {

    }
}
