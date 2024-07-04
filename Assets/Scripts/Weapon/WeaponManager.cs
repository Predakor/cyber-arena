using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour {

    [SerializeField] List<GameObject> weapons;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] PlayerInputHandler PlayerInputHandler;

    [SerializeField] Transform weaponTransform;


    [Header("Events")]
    public UnityEvent onWeaponPickup;
    public UnityEvent onWeaponChange;
    public UnityEvent onWeaponEquiped;

    void Start() {
        if (currentWeapon == null) {
            currentWeapon = gameObject.GetComponentInChildren<Weapon>();
            if (currentWeapon == null) {
                Debug.LogError("No weapon selected ");
            }
        }
        weapons[0] = currentWeapon.gameObject;

        PlayerInputHandler = PlayerInputHandler.Instance;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SwapWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SwapWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SwapWeapon(2);
        }
        if (PlayerInputHandler.ShootInput == 1) {
            currentWeapon.Fire();
        }
    }

    GameObject InstantiateWeapon(GameObject weapon) {
        GameObject instantiedWeapon = Instantiate(weapon, weaponTransform.position, weaponTransform.rotation, weaponTransform);
        instantiedWeapon.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        return instantiedWeapon;
    }

    public void EquipWeapon(GameObject selectedWeapon) {
        if (weapons.Contains(selectedWeapon)) {
            currentWeapon.gameObject.SetActive(false);
            selectedWeapon.SetActive(true);
            currentWeapon = selectedWeapon.GetComponent<Weapon>();
            return;
        }

        GameObject newGun = InstantiateWeapon(selectedWeapon);
        currentWeapon = newGun.GetComponent<Weapon>();

        onWeaponEquiped.Invoke();
    }

    public void SwapWeapon(int number) {
        EquipWeapon(weapons[number]);
        onWeaponChange.Invoke();
    }

    public void PickupNewWeapon(GameObject newWeapon) {
        if (weapons.Count < 3) {
            GameObject newGun = InstantiateWeapon(newWeapon);
            newGun.SetActive(weapons.Count == 0);
            weapons.Add(newGun);
            onWeaponPickup.Invoke();

        }

        //else show menu which player can pick which weapon he wants to replace
    }
}
