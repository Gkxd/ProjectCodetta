using UnityEngine;
using System.Collections;

public class FrameRateDisplay : MonoBehaviour {

    GUIStyle style;
    Rect rect;
    string text = "";

    float fps = 60;

    void Awake() {
        Application.targetFrameRate = -1;
    }

    void Start() {
        int w = Screen.width, h = Screen.height;

        style = new GUIStyle();

        rect = new Rect(0, 0, w, 20);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 50;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    }

    void Update() {
        fps -= fps / 120f;
        fps += (1 / Time.deltaTime) / 120f;
    }


    void OnGUI() {
        GUI.Label(rect, "" + fps, style);
    }
}
