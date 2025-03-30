using UnityEngine;

public class PlayerTracker : MonoBehaviour {
    [SerializeField] Transform _player;
    [SerializeField] bool _isFollowing = true;

    public bool IsFollowing {
        get => _isFollowing;
        set => _isFollowing = value;
    }

    public void Follow() => IsFollowing = true;
    public void UnFollow() => IsFollowing = false;

    void Awake() {
        _player = InstanceHolder.GetPlayer().transform;
    }

    void LateUpdate() {
        if (IsFollowing) {
            transform.position = _player.position;
        }
    }
}
