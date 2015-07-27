using UnityEngine;
using System.Collections;

public class CombatTrigger : MonoBehaviour {

    public GameObject trigger;
    public float radius;

    public Transform spiderSpawn1;
    public Transform spiderSpawn2;
    public Transform spiderSpawn3;

    private bool triggered = false;

	void Update () {
        if (!triggered) {
            if ((transform.position - trigger.transform.position).sqrMagnitude < radius * radius) {
				transform.LookAt(new Vector3(trigger.transform.position.x,transform.position.y,trigger.transform.position.z));
				ModeSwitch.triggerCombatMode(this);
                triggered = true;
            }
        }
	}
}
