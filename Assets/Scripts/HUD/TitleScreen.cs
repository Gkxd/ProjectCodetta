using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	public Transform smartphone;
	public GameObject screen;

	public Texture creditScreen;
	public Texture[] screens;
	private int screenTex;
	private bool touched;
	private float screenY = 0;

	public Vector3 rotAll;
	public Vector3 rotSpd;
	public Vector3 rotAmp;

	public string level;

	public float creditSpeed = 0.5f;
	public static bool credits = false;
   
	void Start () {
		if(credits){ 
			screenY = 0.7f;
      		screenTex = -1; 
			screen.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f,.33f);
			screen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,screenY);
		} else {
			screenY = 0;
      		screenTex = 0;
      		screen.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f,1f);
			screen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,0f);
    	}
  }
  
  	void Update () {

		transform.Rotate(rotAll);
		Vector3 r = rotSpd*Time.time;
		smartphone.localEulerAngles = new Vector3(rotAmp.x*Mathf.Sin(r.x),rotAmp.y*Mathf.Sin(r.y),rotAmp.z*Mathf.Sin(r.z));

		//start game upon touch.
        if(Input.touchCount > 0 || Input.GetMouseButtonDown(0) ) {
			if (!touched) {
				screen.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f,1f);
				screen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,0f);
				credits = false;
				touched = true;
				screenTex++;
				if(screenTex == screens.Length) {
					FadeIn.fadeToOut = true;
        		}
			}
		} else {
			touched = false;
		}
		//display screen
		if(0 <= screenTex && screenTex < screens.Length) {
			screen.GetComponent<Renderer>().material.mainTexture = screens[screenTex];
          	screen.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f,1f);
			screen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,0f);
        } else if (screenTex == -1) {
			screenY -= Time.deltaTime*creditSpeed;
          	screen.GetComponent<Renderer>().material.mainTexture = creditScreen;
			screen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f,screenY);
		}
    	//load next level
		if(FadeIn.fadeToOut && FadeIn.fadeLevel >= 1f) {
			Application.LoadLevel(level);
		}
  	}	
}