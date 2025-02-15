using UnityEngine;

public class ShieldBotAI : GeneralHostileAi {

    public void StartAttack(bool state = true) {
        animator.SetBool("Attacking", state);
    }

    public void RaiseShield(bool state = true) {
        animator.SetBool("Shielding", state);
    }

    [ContextMenu("defences/raise shields")]
    public void TogleShield() {
        RaiseShield(true);
    }

    [ContextMenu("attacks/start attack")]
    public void TogleAttacking() {
        StartAttack(true);
    }


}
