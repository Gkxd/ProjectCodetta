using UnityEngine;
using System.Collections;

public class HeroCombat_Cadence : _HeroCombat {

    protected override void die() {
        Debug.Log("Cadence has died.");
        animator.SetBool("died", true);
    }

    protected sealed override void specialMove(_CombatParticipant other) {
        if (currentMp >= 10) {
            useMP(10);

            int healAmount = Random.Range(minAttack, maxAttack * 2);
            other.heal(healAmount);

            animator.SetTrigger("heal");
            combatController.waitForAnimationFinished(animator);
        }
        else {
            Debug.Log("Cadence does not have enough MP. Skipping turn...");
        }
    }

    public override bool hasEnoughMp() {
        return currentMp >= 10;
    }
}
