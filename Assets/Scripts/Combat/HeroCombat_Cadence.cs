using UnityEngine;
using System.Collections;

public class HeroCombat_Cadence : _HeroCombat {

    protected override void die() {
        Debug.Log("Cadence has died.");
    }

    protected sealed override void specialMove(_CombatParticipant other) {
        if (currentMp >= 10) {
            currentMp -= 10;

            int healAmount = Random.Range(minAttack, maxAttack * 2);
            other.heal(healAmount);
        }
        else {
            Debug.Log("Cadence does not have enough MP. Skipping turn...");
        }
    }
}
