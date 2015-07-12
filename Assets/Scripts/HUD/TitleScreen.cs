using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	public Transform smartphone;
	public GameObject screen;

	public Texture[] screens;
	private int screenTex;
	private bool touched;

	public Vector3 rotAll;
	public Vector3 rotSpd;
	public Vector3 rotAmp;
	
	void Start () {
		screenTex = 0;
	}

	void Update () {

		transform.Rotate(rotAll);
		Vector3 r = rotSpd*Time.time;
		smartphone.localEulerAngles = new Vector3(rotAmp.x*Mathf.Sin(r.x),rotAmp.y*Mathf.Sin(r.y),rotAmp.z*Mathf.Sin(r.z));

		//start game upon touch.
        if(!touched && Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
			touched = true;
			screenTex++;
			if(screenTex == screens.Length) {
				Application.LoadLevel("Loading");
			}
		} else {
			touched = false;
		}
		//display screen
		if(0 <= screenTex && screenTex < screens.Length) {
			screen.GetComponent<Renderer>().material.mainTexture = screens[screenTex];
		}
	}	
}