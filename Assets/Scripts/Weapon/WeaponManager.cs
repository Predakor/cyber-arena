using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour {

    [SerializeField] List<GameObject> weapons;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] Animator animator;

    [SerializeField] Transform weaponTransform;

    [Header("Options")]
    [SerializeField] bool autoEquipWeapon = false;

    float _lastShotTime = 0;

    public static WeaponManager instance;

    [Header("Events")]
    public UnityEvent onWeaponPickup;
    public UnityEvent onWeaponChange;
    public UnityEvent onWeaponEquiped;

    public Weapon CurrentWeapon { get => currentWeapon; }

    void Awake() {
        if (instance == null) { instance = this; }
        if (!animator) {
            if (gameObject.TryGetComponent(out animator)) {
                Debug.LogWarning($"No animator asigned in {gameObject}");
            }
            animator = GetComponent<Animator>();
        }
    }
    void Start() {
        if (currentWeapon == null) {
            currentWeapon = gameObject.GetComponentInChildren<Weapon>();
            if (currentWeapon == null) {
                Debug.LogError("No weapon selected ");
            }
        }
        weapons[0] = currentWeapon.gameObject;
        playerInputHandler = PlayerInputHandler.Instance;
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
        if (playerInputHandler.ShootInput == 1) {
            animator.SetBool("Shooting", true);
            _lastShotTime = Time.time;
            currentWeapon.Fire();
        }
        else {
            if (Time.time - _lastShotTime > 2) {
                animator.SetBool("Shooting", false);
            }
        }
    }


    #region helpers
    GameObject InstantiateWeapon(GameObject weapon) {
        GameObject instantiedWeapon = Instantiate(weapon, weaponTransform.position, weaponTransform.rotation);
        instantiedWeapon.transform.SetParent(weaponTransform, true);
        instantiedWeapon.SetActive(false);

        return instantiedWeapon;
    }
    void SwitchOtherWeapons(GameObject newWeapon) {
        weapons.ForEach(weapon => weapon.SetActive(weapon == newWeapon));
    }
    public void SwapWeapon(int number) {
        EquipWeapon(weapons[number]);
        onWeaponChange.Invoke();
    }
    #endregion

    public void EquipWeapon(GameObject selectedWeapon) {
        if (weapons.Contains(selectedWeapon)) {
            SwitchOtherWeapons(selectedWeapon);
            currentWeapon = selectedWeapon.GetComponent<Weapon>();
            return;
        }

        GameObject newGun = InstantiateWeapon(selectedWeapon);
        currentWeapon = newGun.GetComponent<Weapon>();
        onWeaponEquiped.Invoke();
    }

    public void PickupNewWeapon(GameObject newWeapon) {
        void _CreateWeapon() {
            GameObject newGun = InstantiateWeapon(newWeapon);
            weapons.Add(newGun);
        }

        //no weapon
        if (!weapons.Contains(gameObject)) {
            _CreateWeapon();
            newWeapon.SetActive(true);
            return;
        }

        //to many weapons
        if (weapons.Count == 3) {
            //else show menu which player can pick which weapon he wants to replace
            return;
        }

        GameObject _weapon;
        for (int i = 0; i < weapons.Count; i++) {
            _weapon = weapons[i];
            if (_weapon == null) {
                _CreateWeapon();
                if (autoEquipWeapon) {
                    SwitchOtherWeapons(_weapon);
                }
                break;
            }
        }

    }
}
