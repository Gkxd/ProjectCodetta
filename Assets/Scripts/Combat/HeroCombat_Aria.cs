using UnityEngine;
using System.Collections;

public class HeroCombat_Aria : _HeroCombat {

    protected override void die() {
        Debug.Log("Aria has died.");
    }

    protected sealed override void specialMove(_CombatParticipant other) {
        if (currentMp >= 10) {
            currentMp -= 10;

            int damageAmount = Random.Range(minAttack * 2, maxAttack * 3);
            other.damage(damageAmount);
        }
        else {
            Debug.Log("Aria does not have enough MP. Skipping turn...");
        }
    }
}
