using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour {
    [SerializeField] Animator animator;
    void Start() {
        if (animator == null) { Debug.LogError("No animator asigned", this); }
    }

    public void SetBoolParameter(string paramName, bool value) {
        animator.SetBool(paramName, value);
    }

    public void SetIntParameter(string paramName, int value) {
        animator.SetInteger(paramName, value);
    }

    public void SetFloatParameter(string paramName, float value) {
        animator.SetFloat(paramName, value);
    }

    public void SetTriggerParameter(string paramName) {
        animator.SetTrigger(paramName);
    }

    public void ResetTriggerParameter(string paramName) {
        animator.ResetTrigger(paramName);
    }
}
