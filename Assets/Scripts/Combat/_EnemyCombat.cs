﻿using UnityEngine;
using System.Collections;

public abstract class _EnemyCombat : _CombatParticipant {
    protected override void die() {
        animator.SetTrigger("Die");
    }
}
