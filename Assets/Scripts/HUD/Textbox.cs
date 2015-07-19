using UnityEngine;
using System.Collections;

public class Textbox : MonoBehaviour {

    public enum Speaker { ARIA = 0, BRIO = 1, CADENCE = 2 };

    public GameObject aria, brio, cadence;

    private GameObject[] speakers;
	void Start () {
        speakers = new GameObject[3];
        speakers[0] = aria;
        speakers[1] = brio;
        speakers[2] = cadence;
	}
	
	void Update () {
	
	}

    public void setSpeaker(Speaker speaker) {
        speakers[(int)speaker].SetActive(true);
        speakers[(((int)speaker)) + 1 % 3].SetActive(true);
        speakers[(((int)speaker)) + 2 % 3].SetActive(true);
    }

    public void setText(string text) {

    }
}
