using UnityEngine;

public class WeaponManager : MonoBehaviour {

    [SerializeField] Weapon weapon;
    [SerializeField] PlayerInputHandler PlayerInputHandler;

    // Start is called before the first frame update
    void Start() {

        if (weapon == null) { Debug.LogError("No weapon selected "); }
        PlayerInputHandler = PlayerInputHandler.Instance;
    }

    // Update is called once per frame
    void Update() {

        if (PlayerInputHandler.ShootInput == 1) {
            weapon.Fire();
        }
    }
}
