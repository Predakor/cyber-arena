using UnityEngine;

public class WeaponManager : MonoBehaviour {

    [SerializeField] Weapon weapon;
    [SerializeField] PlayerInputHandler PlayerInputHandler;

    [SerializeField] Transform weaponTransform;

    // Start is called before the first frame update
    void Start() {
        if (weapon == null) {
            weapon = gameObject.GetComponentInChildren<Weapon>();
            if (weapon == null) {
                Debug.LogError("No weapon selected ");
            }
        }

        PlayerInputHandler = PlayerInputHandler.Instance;
    }

    // Update is called once per frame
    void Update() {

        if (PlayerInputHandler.ShootInput == 1) {
            weapon.Fire();
        }
    }

    public void SetActiveWeapon(GameObject newWeapon) {
        GameObject newGun = Instantiate(newWeapon, weaponTransform.position, Quaternion.identity, weaponTransform);
        weapon = newGun.GetComponent<Weapon>();
    }
}
