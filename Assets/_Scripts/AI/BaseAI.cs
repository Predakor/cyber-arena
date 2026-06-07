using Assets.Scripts.Utils;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(TargetMovement))]
public class BaseAI : MonoBehaviour
{
    [Header("Base references")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected TargetMovement movement;

    protected virtual void Awake()
    {
        gameObject
            .EnsureComponent(out animator)
            .EnsureComponent(out movement);

    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
}
