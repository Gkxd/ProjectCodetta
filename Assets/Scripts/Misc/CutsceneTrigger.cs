using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {

    public GameObject trigger;
    public float radius;

    public Cutscene cutscene;

    private bool hasTriggered = false;

    void Update() {
        if (!hasTriggered) {
            if ((trigger.transform.position - transform.position).sqrMagnitude < radius * radius) {
                CutsceneController.setCutscene(cutscene);
                hasTriggered = true;
            }
        }
    }
}
