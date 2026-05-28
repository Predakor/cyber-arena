using Assets.Scripts.Utils;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Start()
    {
        gameObject.EnsureComponent(out animator);
    }

    public void SetBoolParameter(string paramName, bool value)
    {
        animator.SetBool(paramName, value);
    }

    public void SetIntParameter(string paramName, int value)
    {
        animator.SetInteger(paramName, value);
    }

    public void SetFloatParameter(string paramName, float value)
    {
        animator.SetFloat(paramName, value);
    }

    public void SetTriggerParameter(string paramName)
    {
        animator.SetTrigger(paramName);
    }

    public void ResetTriggerParameter(string paramName)
    {
        animator.ResetTrigger(paramName);
    }
}
