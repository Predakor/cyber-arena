using UnityEngine;
using UnityEngine.Events;

public class GeneralHostileAi : BaseAI
{
    [SerializeField] private GameObject target;
    [SerializeField] private Collider _agroCollider;

    [Header("AI Flags")]
    [SerializeField] private bool _triggered = false;

    [Header("Weapons")]
    [SerializeField] private SimpleWeapon[] _weapons;

    [Header("Events")]
    public UnityEvent OnAggro;
    public UnityEvent OnAggroLost;
    public UnityEvent OnTrigger;
    public UnityEvent OnTargetAcquired;
    public UnityEvent OnTargetLost;

    public virtual void Trigger()
    {
        SetTarget(target);
        OnTrigger?.Invoke();
    }

    public virtual void Attack() { }

    public virtual void SetTarget(GameObject newTarget)
    {
        if (newTarget == null)
        {
            OnTargetLost?.Invoke();
        }
        if (newTarget != target)
        {
            OnTargetAcquired?.Invoke();
        }
        ChangeTarget(newTarget);
    }

    private void ChangeTarget(GameObject newTarget)
    {
        if (newTarget == null)
        {
            _triggered = false;
            target = null;
            movement.SetMovementTarget(null);
            movement.RotateTowards(null);
            return;
        }

        _triggered = true;
        target = newTarget;
        movement.SetMovementTarget(target.transform);
        movement.RotateTowards(target.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _triggered = true;
            ChangeTarget(other.gameObject);
            OnAggro?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggered && target == other.gameObject)
        {
            _triggered = false;
            ChangeTarget(null);
            OnAggroLost?.Invoke();
        }
    }

    protected override void Update()
    {
        if (!_triggered || target == null)
        {
            return;
        }

        //check line sight to player don't shoot otherwise
        foreach (var weapon in _weapons)
        {
            weapon.TryFire();
        }
    }
}


