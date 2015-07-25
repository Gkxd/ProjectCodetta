using UnityEngine;
using System.Collections;


[System.Serializable]
public class CutsceneStep {
    public Textbox.Speaker speaker;
    public string text;
}

[System.Serializable]
public class Cutscene {
    public CutsceneStep[] cutsceneSteps;
}

public class CutsceneController : MonoBehaviour {

    public static CutsceneController instance;

    public Textbox textBox;

    private Cutscene currentCutscene;
    private int cutsceneStep;

    void Awake() {
        instance = this;
    }

	void Update () {

        if (ModeSwitch.isCutscene()) {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Z)) {
#else
            if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
#endif
                advanceCutscene();
            }
        }
	}

    private void advanceCutscene() {
        cutsceneStep++;

        if (cutsceneStep < currentCutscene.cutsceneSteps.Length) {
            CutsceneStep step = currentCutscene.cutsceneSteps[cutsceneStep];
            textBox.setText(step.text, step.speaker);
        }
        else {
            ModeSwitch.switchCutsceneOff();
        }
    }

    public static void setCutscene(Cutscene cutscene) {
        ModeSwitch.switchCutsceneOn();

        instance.currentCutscene = cutscene;
        instance.cutsceneStep = 0;

        CutsceneStep step = instance.currentCutscene.cutsceneSteps[0];
        instance.textBox.setText(step.text, step.speaker);
    }
}
