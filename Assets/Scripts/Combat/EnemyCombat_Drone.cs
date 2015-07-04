using UnityEngine;
using System.Collections;

public class EnemyCombat_Drone : _EnemyCombat {
    protected override void die() {
        Debug.Log(gameObject.name + " has died.");
    }
}
