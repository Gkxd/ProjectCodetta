﻿using UnityEngine;
using System.Collections;

public class HeroCombat_Brio : _HeroCombat {

    protected override void die() {
        Debug.Log("Brio has died.");
    }

    protected sealed override void specialMove(_CombatParticipant other) {
        if (currentMp >= 20) {
            useMP(20);

            int damageAmount = Random.Range(minAttack + maxAttack, maxAttack * 3);
            other.damage(damageAmount);
        }
        else {
            Debug.Log("Brio does not have enough MP. Skipping turn...");
        }
    }
    public override bool hasEnoughMp() {
        return currentMp >= 20;
    }
}
