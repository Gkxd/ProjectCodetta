using UnityEngine;
using System.Collections;

public class Textbox : MonoBehaviour {

    public enum Speaker { ARIA = 0, BRIO = 1, CADENCE = 2 };

    public GameObject aria, brio, cadence;
    public Color[] speakerColors;
    public TextMesh textMesh;

    private GameObject[] speakers;
	void Awake () {
        speakers = new GameObject[3];
        speakers[0] = aria;
        speakers[1] = brio;
        speakers[2] = cadence;
	}

    public void setText(string text, Speaker speaker) {
        textMesh.text = text;
        setSpeaker(speaker);
    }

    public void setText(string text) {
        textMesh.text = text;
    }

    private void setSpeaker(Speaker speaker) {
        speakers[(int)speaker].SetActive(true);
        speakers[(((int)speaker) + 1) % 3].SetActive(false);
        speakers[(((int)speaker) + 2) % 3].SetActive(false);

        textMesh.color = speakerColors[(int)speaker];
    }
}
