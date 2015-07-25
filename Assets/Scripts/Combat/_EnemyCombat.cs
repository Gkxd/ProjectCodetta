using UnityEngine;
using System.Collections;

public abstract class _EnemyCombat : _CombatParticipant {

    protected override void die() {
        animator.SetTrigger("Die");
    }

    public void selectEnemy() {
        hpBar.parent.transform.localScale = new Vector3(1, 1, 1);
    }

    public void deselectEnemy() {
        hpBar.parent.transform.localScale = new Vector3(0.25f, 0.25f, 1);
    }
}
