using UnityEngine;
using System.Collections;

public class HeroCombat_Aria : _HeroCombat {

    protected override void die() {
        Debug.Log("Aria has died.");

        // Game over
    }

    protected sealed override void specialMove(_CombatParticipant other) {
        if (currentMp >= 10) {
            useMP(10);

            int damageAmount = Random.Range(minAttack * 2, maxAttack * 3);
            other.damage(damageAmount);

            animator.SetTrigger("sword");
            //combatController.waitForAnimationFinished(animator);
            combatController.waitForParticipant(this, 4);
        }
        else {
            Debug.Log("Aria does not have enough MP. Skipping turn...");
        }
    }
    public override bool hasEnoughMp() {
        return currentMp >= 10;
    }
}
