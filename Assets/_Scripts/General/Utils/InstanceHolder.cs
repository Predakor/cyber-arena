using UnityEngine;

public class InstanceHolder : Singleton<InstanceHolder> {
    [SerializeField] GameObject _player;
    [SerializeField] Transform _mouseTracker;
    public GameObject Player {
        get {
            if (_player == null) {

                _player = GameObject.FindWithTag("Player");
            }
            return _player;
        }
    }

    public Transform MouseTracker {
        get {
            if (_mouseTracker == null) {
                _mouseTracker = FindObjectOfType<FollowMouse>().transform;
            }
            return _mouseTracker;
        }
    }

    public static GameObject GetPlayer() => Instance.Player;
}
