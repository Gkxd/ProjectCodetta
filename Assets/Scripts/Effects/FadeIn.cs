using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	public Texture black;
	public float fadeInSpeed = 0.5f;
	public float fadeOutSpeed = 0.5f;
	public static float fadeLevel = 1;
	public static bool fadeToOut = false;
	void Start() {
		fadeLevel = 1;
		fadeToOut = false;
		Cursor.visible = false;
	}
	void OnGUI () {
		GUI.depth = 0;
		GUI.color = new Color (1, 1, 1, fadeLevel);
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), black, ScaleMode.StretchToFill, true);
		if(!fadeToOut) {
			if (fadeLevel > 0) { fadeLevel -= fadeInSpeed * Time.deltaTime; }
			else { fadeLevel = 0; }
		} else {
			if (fadeLevel < 1) { fadeLevel += fadeOutSpeed * Time.deltaTime; }
			else { fadeLevel = 1; }
		}
	}
}
