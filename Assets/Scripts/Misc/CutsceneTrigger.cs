using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {

    public GameObject trigger;
    public float radius;

    public Cutscene cutscene;

    private bool hasTriggered = false;
	public string level;

    void Update() {
        if (!hasTriggered) {
            if ((trigger.transform.position - transform.position).sqrMagnitude < radius * radius) {
                if(level == "") {
					CutsceneController.setCutscene(cutscene);
					hasTriggered = true;
				} else {
					FadeIn.fadeToOut = true;
				}
            }
        }
		if(level != "") {
        	if(FadeIn.fadeToOut && FadeIn.fadeLevel >= 1) {
		        Application.LoadLevel(level);
			}
		}
	}
}
