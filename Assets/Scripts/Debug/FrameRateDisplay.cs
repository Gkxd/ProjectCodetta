using UnityEngine;
using System.Collections;

public class FrameRateDisplay : MonoBehaviour {

    GUIStyle style;
    Rect rect;
    string text = "";

    float fps;

    void Start() {
        int w = Screen.width, h = Screen.height;

        style = new GUIStyle();

        rect = new Rect(0, 0, w, 20);
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    }

    void Update() {
        fps = 1f/Time.deltaTime;
    }


    void OnGUI() {
        GUI.Label(rect, "" + fps, style);
    }
}
