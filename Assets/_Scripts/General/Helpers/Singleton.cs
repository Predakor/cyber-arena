using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    public static T Instance { get; private set; }

    virtual protected void Awake() {
        if (Instance == null) {
            Instance = this as T;
        }
    }


}
