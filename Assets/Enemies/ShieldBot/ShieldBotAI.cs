using UnityEngine;

public class ShieldBotAI : GeneralHostileAi {

    #region variables
    bool _isShieldUp = false;
    bool _isAttacking = false;

    public bool IsShieldUp {
        get => _isShieldUp; private set {
            _isShieldUp = value;
            animator.SetBool("Shielding", value);
        }
    }

    public bool IsAttacking {
        get => _isAttacking; private set {
            _isAttacking = value;
            animator.SetBool("Attacking", value);
        }
    }

    #endregion

    public override void Trigger() {
        RaiseShield();
        TogleAttacking();
        movement.AllowMovement();
        movement.AllowRotation();
        base.Trigger();
    }

    [ContextMenu("defences/raise shields")]
    public void RaiseShield(bool state = true) {
        IsShieldUp = state;
    }

    [ContextMenu("attacks/start attack")]
    public void TogleAttacking(bool state = true) {
        IsAttacking = state;
    }




}
