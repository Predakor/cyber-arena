using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]

public class Door : MonoBehaviour {

    Animator animator;
    [SerializeField] float _openingSpeed = 1f;
    [SerializeField] Collider _collider;

    bool isLocked = false;
    [SerializeField] bool isOpen = false;
    [SerializeField] float animationTime = .5f;

    public UnityEvent<bool> OnAnimationEnd;

    public bool IsOpen {
        get => isOpen; private set {
            if (IsLocked) return;
            animator.SetBool("Open", value);
            StartCoroutine(WaitForAnimation(value));
        }
    }

    public bool IsLocked { get => isLocked; private set => isLocked = value; }
    public void UnlockDoor() => IsLocked = false;
    public void LockDoor() => IsLocked = true;
    public void OpenDoor() => IsOpen = true;
    public void CloseDoor() => IsOpen = false;


    void OnCollisionEnter(Collision collision) {
        if (isLocked) {
            return;
        }

        OpenDoor();
    }

    IEnumerator WaitForAnimation(bool state) {
        yield return new WaitForSeconds(animationTime);
        _collider.enabled = !state;
        isOpen = state;
        OnAnimationEnd?.Invoke(state);
    }

    void Awake() {
        if (animator == null) { animator = GetComponent<Animator>(); }
        if (_collider == null) { _collider = GetComponent<Collider>(); }
        animator.speed = _openingSpeed;

    }

}
