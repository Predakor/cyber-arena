using UnityEngine;

public class BaseAI : MonoBehaviour {

    [Header("Base references")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected TargetMovement movement;
    [SerializeField] protected Health health;

    virtual protected void Awake() {
        if (animator == null) { animator = GetComponent<Animator>(); }
        if (movement == null) { movement = GetComponent<TargetMovement>(); }
        if (health == null) { health = GetComponent<Health>(); }

    }
    virtual protected void Start() {
        if (animator == null) { Debug.LogError("no animator in", this); }
        if (movement == null) { Debug.LogWarning("no movement in", this); }
        if (health == null) { Debug.Log("no health in", this); }
    }

    virtual protected void Update() {

    }
}
