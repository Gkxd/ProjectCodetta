using UnityEngine;
using System.Collections;

public class BossBattleTrigger : MonoBehaviour {

    public GameObject trigger;
    public float radius;

    private bool triggered = false;

    void Update() {
        if (!triggered) {
            if ((transform.position - trigger.transform.position).sqrMagnitude < radius * radius) {
                ModeSwitch.triggerBossBattle(this);

                transform.Find("HP Billboard").gameObject.SetActive(true);

                triggered = true;
            }
        }
    }
}
