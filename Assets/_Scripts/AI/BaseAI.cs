using Assets.Scripts.Utils;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(TargetMovement), typeof(Health))]
public class BaseAI : MonoBehaviour
{
    [Header("Base references")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected TargetMovement movement;
    [SerializeField] protected Health health;

    protected virtual void Awake()
    {
        gameObject
            .EnsureComponent(out animator)
            .EnsureComponent(out movement)
            .EnsureComponent(out health);

    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
}
