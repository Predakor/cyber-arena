using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour {

    public TextMeshProUGUI ammoCountDisplay;
    public static AmmoDisplay instance;

    void Start() {
        if (instance == null) {
            instance = this;
        }
    }

    public void SetAmmoText(float ammoCount) {
        if (ammoCountDisplay) {
            ammoCountDisplay.text = ammoCount.ToString();

        }
    }
    public void SetAmmoText(int ammoCount) {
        if (ammoCountDisplay) {
            ammoCountDisplay.text = ammoCount.ToString();

        }
    }
}
